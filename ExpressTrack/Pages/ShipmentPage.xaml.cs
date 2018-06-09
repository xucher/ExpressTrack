using ExpressTrack.DB;
using ExpressTrack.Libs;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ExpressTrack {
    public partial class ShipmentPage : Page {
        public const int INSTOCK = 0;
        public const int OUTSTOCK = 1;
        private int type;
        public ShipmentPage(int type) {
            this.type = type;
            InitializeComponent();
            initUI();
            setDataContext();
        }

        private FixedReader mReader = new FixedReader();
        private DispatcherTimer mTimer;

        private void initUI() {
            if (type == INSTOCK) {
                labTitle.Content = "快递入库单";
                labListCoding.Content = "入库单号";
                columnFromStation.Header = "发自站点";
            } else if (type == OUTSTOCK) {
                labTitle.Content = "快递出库单";
                labListCoding.Content = "出库单号";
                columnFromStation.Header = "发往站点";
            }
        }

        private void setDataContext() {
            var viewModel = new ShipmentViewModel {
                InStockId = "I0001159",
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
            ShipmentViewModel mShipmentModel = DataContext as ShipmentViewModel;
            var express = MySqlHelper.getExpressByCoding(coding);
            if (express != null) {
                string station = "";
                if (type == INSTOCK) {
                    station = MySqlHelper.getFromStationByExpress(coding, mShipmentModel.SelectedStation);
                } else if (type == OUTSTOCK) {
                    station = MySqlHelper.getNextStation(coding, mShipmentModel.SelectedStation);
                }
                
                if (station != "") {
                    mShipmentModel.Shipments.Add(new ShipmentModel {
                        Coding = coding,
                        Name = express.Name,
                        Station = station
                    });
                }
            } else {
                Console.WriteLine("编号为" + coding + "的快递不存在");
            }
            Console.WriteLine(mShipmentModel.Shipments.Count);
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
    }
}
