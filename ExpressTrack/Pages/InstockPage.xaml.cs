using ExpressTrack.Libs;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ExpressTrack {
    public partial class InstockPage : Page {
        public InstockPage() {
            InitializeComponent();

            // 连接天线
            //mReader = new FixedReader();
            //mReader.ConnectAnt(FixedReader.ANT1);
            setDataContext();
        }

        private FixedReader mReader;
        private DispatcherTimer mTimer;

        private void setDataContext() {
            var stations = new ObservableCollection<string> { "station1", "station2", "station3" };
            var viewModel = new InStockViewModel {
                StationIds = stations,
                InStockId = "201806031159",
                Shipments = new ObservableCollection<Models.Shipment>(),
                Date = DateTime.Now,
                DeviceState = true
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
                        (DataContext as InStockViewModel).Shipments.Add(
                            new Models.Shipment { FromStation = data.EPC }
                         );
                    }
                });
                mTimer.Interval = new TimeSpan(0, 0, 1);
            }
            mTimer.Start();
            btnEnd.IsEnabled = true;
        }

        // 停止读取
        private void btnEnd_Click(object sender, System.Windows.RoutedEventArgs e) {
            btnEnd.IsEnabled = false;
            mTimer.Stop();
            btnStart.IsEnabled = true;
        }
    }
}
