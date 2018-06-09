using ExpressTrack.DB;
using ExpressTrack.Libs;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ExpressTrack {
    public partial class InstockPage : Page {
        public InstockPage() {
            InitializeComponent();

            setDataContext();
        }

        private FixedReader mReader = new FixedReader();
        private DispatcherTimer mTimer;

        private void setDataContext() {
            var viewModel = new InStockViewModel {
                InStockId = "I0001159",
                NowStation = "StationA",
                CheckDate = DateTime.Now,
                DeviceState = true,
                InstockExpresses = new ObservableCollection<InstockModel>()
            };
            DataContext = viewModel;
        }

        // 开始循环读取数据
        private void btnStart_Click(object sender, System.Windows.RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            if (mTimer == null) {
                mTimer = new DispatcherTimer();
                mTimer.Tick += new EventHandler((s, ev) => {
                    var data = mReader.ReadData();
                    if (data != null) {
                        // TODO: data.Epc需要重新编码
                        geneInstockByCoding(data.EPC, -1);
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

        // 手动输入编号
        private void DG_instock_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.Column.DisplayIndex == 0) {
                geneInstockByCoding((e.EditingElement as TextBox).Text, e.Row.GetIndex());
            }
        }

        private void geneInstockByCoding(string coding, int index) {
            InStockViewModel mInstockModel = DataContext as InStockViewModel;
            var express = MySqlHelper.getExpressByCoding(coding);
            if (express != null) {
                string fromStation = MySqlHelper.getFromStationByExpress(coding, mInstockModel.NowStation);
                if (fromStation != "") {
                    if (Tgb_isAuto.IsChecked == true) {
                        // 手动输入
                        mInstockModel.InstockExpresses[index].Name = express.Name;
                        mInstockModel.InstockExpresses[index].FromStation = fromStation;
                    } else {
                        // 通过读写器输入
                        mInstockModel.InstockExpresses.Add(new InstockModel {
                            Coding = coding,
                            Name = express.Name,
                            FromStation = fromStation
                        });
                    }
                }
            } else {
                Console.WriteLine("编号为" + coding + "的快递不存在");
            }
        }

        private void Tgb_isAuto_Click(object sender, RoutedEventArgs e) {
            if (Tgb_isAuto.IsChecked == true) {
                manualAddPanelColumn.Width = new GridLength(250);
            } else {
                manualAddPanelColumn.Width = new GridLength(0);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e) {
            // 判断要连接的天线
            mReader.ConnectAnt(FixedReader.ANT1);
            btnStart.IsEnabled = true;
        }
    }
}
