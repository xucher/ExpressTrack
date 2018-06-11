using ExpressTrack.DB;
using ExpressTrack.Libs;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;

namespace ExpressTrack {
    public partial class ShipmentPage : Page {
        public const int INSTOCK = 0;
        public const int OUTSTOCK = 1;
        private int type;
        public ShipmentPage(int type) {
            this.type = type;
            InitializeComponent();
            initUI();
            

            inStockStartId = MySqlHelper.getLastInstockIndex() + 1;
            outStockStartId = MySqlHelper.getLastOutstockIndex() + 1;
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
                DeviceState = true,
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
                            data.EPC = "201806090001";
                        } else if (data.EPC == "B002") {
                            data.EPC = "201806090002";
                        } else if (data.EPC == "2000") {
                            data.EPC = "201806090003";
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
            // TODO: 判断出库单状态,更新快递状态
            ShipmentViewModel mShipmentModel = DataContext as ShipmentViewModel;

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
                        } else if (type == OUTSTOCK) {
                            db.Outstock.Add(new Models.Outstock {
                                Coding = mShipmentModel.ShipmentId,
                                ExpressCoding = coding,
                                FromStation = mShipmentModel.SelectedStation,
                                ToStation = station,
                                CheckDate = mShipmentModel.CheckDate.ToString()
                            });
                        }
                    }
                } else {
                    Helpers.showMsg("编号为" + coding + "的快递不存在");
                }
                Console.WriteLine(mShipmentModel.Shipments.Count);
            } else {
                Helpers.showMsg("该快递已经添加过");
            }
        }

        private void Tgb_isAuto_Click(object sender, RoutedEventArgs e) {
            if (Tgb_isAuto.IsChecked == true) {
                btnConnect.IsEnabled = false;
                manualAddPanelColumn.Width = new GridLength(250);
            } else {
                manualAddPanelColumn.Width = new GridLength(0);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e) {
            string ant = "";
            // 判断要连接的天线
            string station = (DataContext as ShipmentViewModel).SelectedStation;
            switch(station) {
                case "StationA":
                    ant = FixedReader.ANT1;
                    break;
                case "StationB":
                    ant = FixedReader.ANT2;
                    break;
                case "StationC":
                    ant = FixedReader.ANT3;
                    break;
                default:
                    ant = FixedReader.ANT3;
                    break;
            }
            mReader.ConnectAnt(ant);
            Helpers.showMsg("连接成功");
            btnStart.IsEnabled = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            geneShipmentByCoding(tbxAdd.Text.Trim());
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
