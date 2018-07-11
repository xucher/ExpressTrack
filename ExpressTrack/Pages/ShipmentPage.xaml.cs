using ExpressTrack.DB;
using ExpressTrack.Libs;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.IO;
using ExpressTrack.Models;

namespace ExpressTrack {
    public partial class ShipmentPage : Page {
        public const int INSTOCK = 0;
        public const int OUTSTOCK = 1;
        private int type;
        public ShipmentPage(int type) {
            this.type = type;
            InitializeComponent();
            initUI();

            if (MySqlHelper.getLastInstock() != null) {
                inStockStartId = Helpers.parseShipmentCoding(MySqlHelper.getLastInstock().Coding) + 1;
            } else {
                inStockStartId = 1;
            }
            if (MySqlHelper.getLastOutstock() != null) {
                outStockStartId = Helpers.parseShipmentCoding(MySqlHelper.getLastOutstock().Coding) + 1;
            } else {
                outStockStartId = 1;
            }
            setDataContext();
        }

        private FixedReader mReader = new FixedReader();
        private DispatcherTimer mTimer;
        private ExpressDBContext db = new ExpressDBContext();
        private int inStockStartId;
        private int outStockStartId;

        private void initUI() {
            if (type == INSTOCK) {
                labTitle.Content = "快递入库单";
                labListCoding.Content = "入库单号：";
                columnFromStation.Header = "发自站点";
            } else if (type == OUTSTOCK) {
                labTitle.Content = "快递出库单";
                labListCoding.Content = "出库单号：";
                columnFromStation.Header = "发往站点";
            }
        }

        private void setDataContext() {
            string shipmentId = "";
            if (type == INSTOCK) {
                shipmentId = Helpers.convertShipmentCoding(inStockStartId, INSTOCK);
            } else {
                shipmentId = Helpers.convertShipmentCoding(outStockStartId, OUTSTOCK);
            }
            var viewModel = new ShipmentViewModel {
                ShipmentId = shipmentId,
                Stations = new ObservableCollection<string>(MySqlHelper.getAllStationName()),
                CheckDate = DateTime.Now,
                SelectedStation = Helpers.getStationByPosition(),
                DeviceState = false,
                Shipments = new ObservableCollection<ShipmentModel>()
            };
            DataContext = viewModel;
        }

        // 开始循环读取数据
        private void btnStart_Click(object sender, RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            if (mTimer == null) {
                mTimer = new DispatcherTimer();
                mTimer.Tick += new EventHandler((s, ev) => {
                    var data = mReader.ReadData();
                    if (data != null) {
                        Console.WriteLine(data.EPC);
                        // TODO: data.Epc需要重新编码
                        if (data.EPC == "1234") {
                            data.EPC = Helpers.convertExpressCoding(1);
                        } else if (data.EPC == "B002") {
                            data.EPC = Helpers.convertExpressCoding(2);
                        } else {
                            data.EPC = Helpers.convertExpressCoding(3);
                        }

                        geneShipmentByCoding(data.EPC);
                    }
                });
                mTimer.Interval = new TimeSpan(0, 0, 1);
            }
            mTimer.Start();
            btnEnd.IsEnabled = true;
        }

        // 停止读取
        private void btnEnd_Click(object sender, RoutedEventArgs e) {
            btnEnd.IsEnabled = false;
     
            mTimer.Stop();
            mTimer = null;

            btnStart.IsEnabled = true;
        }

        private void geneShipmentByCoding(string coding) {
            ShipmentViewModel mShipmentModel = DataContext as ShipmentViewModel;
            // 判断中转站是否已选
            if (mShipmentModel.SelectedStation != null) {
                // 判断是否已经添加过
                var query = from s in mShipmentModel.Shipments
                            where s.Coding == coding
                            select s;
                if (query.Count() == 0) {
                    if (btnSave.IsEnabled == false) {
                        btnSave.IsEnabled = true;
                    }
                    var express = MySqlHelper.getExpressByCoding(coding);
                    if (express != null) {
                        string station = "";
                        int flag = 0;

                        if (express.State == (int)ExpressListPage.ExpressState.DELIVERING) {
                            station = MySqlHelper.getLastOutstock().ToStation;
                            if (station != mShipmentModel.SelectedStation) {
                                Helpers.showMsg("快递应该在 " + station + " 入库");
                                flag = 1;
                            }
                        }
                        if (express.State == (int)ExpressListPage.ExpressState.INSTOCK) {
                            station = MySqlHelper.getLastInstock().ToStation;
                            if (station != mShipmentModel.SelectedStation) {
                                Helpers.showMsg("快递应该在 " + station + " 出库");
                                flag = 1;
                            }
                        }
                        if (express.State == (int)ExpressListPage.ExpressState.FINISH) {
                            Helpers.showMsg("快递即将到达代收点");
                            flag = 1;
                        }

                        if (flag == 0) {
                            if (type == INSTOCK) {
                                station = MySqlHelper.getFromStation(coding, mShipmentModel.SelectedStation);
                            } else if (type == OUTSTOCK) {
                                station = MySqlHelper.getToStation(coding, mShipmentModel.SelectedStation);
                            }

                            if (station != "") {
                                mShipmentModel.Shipments.Add(new ShipmentModel {
                                    Coding = coding,
                                    Name = express.Name,
                                    Station = station
                                });
                                if (type == INSTOCK) {
                                    db.Instock.Add(new Models.Instock {
                                        Coding = mShipmentModel.ShipmentId,
                                        ExpressCoding = coding,
                                        FromStation = station,
                                        ToStation = mShipmentModel.SelectedStation,
                                        CheckDate = mShipmentModel.CheckDate.ToString()
                                    });
                                    express.State = (int)ExpressListPage.ExpressState.INSTOCK;
                                    MySqlHelper.changeExpressState(express);
                                } else if (type == OUTSTOCK) {
                                    db.Outstock.Add(new Models.Outstock {
                                        Coding = mShipmentModel.ShipmentId,
                                        ExpressCoding = coding,
                                        FromStation = mShipmentModel.SelectedStation,
                                        ToStation = station,
                                        CheckDate = mShipmentModel.CheckDate.ToString()
                                    });
                                    if (station != "--") {
                                        express.State = (int)ExpressListPage.ExpressState.DELIVERING;
                                        MySqlHelper.changeExpressState(express);
                                    }
                                    if (station == "--") {
                                        express.State = (int)ExpressListPage.ExpressState.FINISH;
                                        MySqlHelper.changeExpressState(express);
                                    }
                                }
                            }
                        }
                    } else {
                        Helpers.showMsg("编号为" + coding + "的快递不存在");
                    }
                    Console.WriteLine(mShipmentModel.Shipments.Count);
                } else {
                    Helpers.showMsg("该快递已经添加过");
                }
            } else {
                Helpers.showMsg("未选择当前中转站");
            }
        } 

        private void Tgb_isAuto_Click(object sender, RoutedEventArgs e) {
            if (Tgb_isAuto.IsChecked == true) {
                btnConnect.IsEnabled = false;
                manualAddPanelColumn.Width = new GridLength(250);

                cbxAdd.ItemsSource = from a in MySqlHelper.getAllExpress()
                                     select a.Coding;
            } else {
                btnConnect.IsEnabled = true;
                manualAddPanelColumn.Width = new GridLength(0);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e) {
            if ((DataContext as ShipmentViewModel).SelectedStation != null) {
                string ant = "";
                // 默认连接10号柜子
                int antIndex = 1;
                // 判断要连接的天线
                string station = (DataContext as ShipmentViewModel).SelectedStation;
                switch (station) {
                    case "StationA":
                        ant = FixedReader.ANT1;
                        break;
                    case "StationB":
                        ant = FixedReader.ANT2;
                        break;
                    case "StationC":
                        ant = FixedReader.ANT3;
                        antIndex = 1;
                        break;
                    case "StationD":
                        ant = FixedReader.ANT3;
                        antIndex = 2;
                        break;
                    case "StationE":
                        ant = FixedReader.ANT3;
                        antIndex = 3;
                        break;
                    case "StationF":
                        ant = FixedReader.ANT3;
                        antIndex = 4;
                        break;
                }

                try {
                    mReader.ConnectAnt(ant, antIndex);
                    Helpers.showMsg("连接成功，读写器可以使用");
                    btnStart.IsEnabled = true;
                    (DataContext as ShipmentViewModel).DeviceState = true;
                } catch (IOException) {
                    Helpers.showMsg("连接失败");
                }
            } else {
                Helpers.showMsg("未选择中转站");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            if (cbxAdd.SelectedValue != null) {
                geneShipmentByCoding(cbxAdd.SelectedValue.ToString());
            } else {
                Helpers.showMsg("未选择编号");
            }
        }

        private void btnAddBatch_Click(object sender, RoutedEventArgs e) {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            mReader.Close();
            mReader = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            if (db.SaveChanges() > 0) {
                Helpers.showMsg("保存成功");
            };
        }
    }
}