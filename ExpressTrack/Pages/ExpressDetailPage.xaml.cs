using ExpressTrack.DB;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WebSocketSharp;

namespace ExpressTrack {
    public partial class ExpressDetailPage : Page {
        public ExpressDetailPage() {
            InitializeComponent();
            DataContext = new ExpressDetailViewModel {
                NowAddress = "未连接设备...",
                //ShipRecords = new ObservableCollection<ExpressDetailViewModel.ShipRecord>(
                //    MySqlHelper.getShipRecord())
            };
        }

        private PathFigure pathFigure;
        private WebSocket ws;
        private Stopwatch timer;
        private int flag = 1;   // 标识是否为第一个数据
        // 处理获得的坐标数据
        private void handleLocalSense(object sender, MessageEventArgs e) {   
            if (!timer.IsRunning) {
                timer.Start();
            }
            
            if (timer.ElapsedTicks > 80*1000) {
                int[] location = Helpers.parseLocalsenseBlob(e.Data);
                if (location[0] == 14913) {
                    // 第一次发送数据
                    if (flag == 1) {
                        Dispatcher.Invoke(new Action(delegate {
                            pathFigure = new PathFigure { StartPoint = new Point(location[1] / 2.4, location[2] / 2.5) };
                            trackGeometry.Figures.Add(pathFigure);
                            
                        }));
                        flag = 0;
                    } else {
                        Dispatcher.Invoke(new Action(delegate {
                            if (pathFigure != null) {
                                pathFigure.Segments.Add(new LineSegment(new Point(location[1] / 2.4, location[2] / 2.5), true));
                            }
                            
                        }));
                    }
                    Dispatcher.Invoke(new Action(delegate {
                        (DataContext as ExpressDetailViewModel).NowAddress =
                                "(" + location[1] + ", " + location[2] + ")";
                    }));
                }
                timer.Reset();
                timer.Start();
            }
        }

        // 开始追踪
        private void btnStart_Click(object sender, RoutedEventArgs e) {
            if (timer == null) {
                timer = new Stopwatch();
            }
            
            initWebsocket();
            btnStart.IsEnabled = false;
            ws.Connect();
            
        }

        // 初始化websocket
        private void initWebsocket() {
            ws = new WebSocket(url: "ws://192.168.0.151:9001", protocols: "localSensePush-protocol");
            
            ws.OnOpen += (sender, e) => {
                btnClose.IsEnabled = true;
                btnReset.IsEnabled = true;
                Console.WriteLine("onOpen: " + ws.ReadyState);
            };
            ws.OnMessage += handleLocalSense;
            ws.OnClose += (sender, e) => {
                Console.WriteLine("onClose: " + ws.ReadyState);
                Console.WriteLine("断开连接");
            };
            ws.OnError += (sender, e) => {
                Console.WriteLine("onError: " + ws.ReadyState);
                Console.WriteLine("Websocket异常，错误信息为" + e.Message);
            };
        }

        // 关闭连接
        private void btnClose_Click(object sender, RoutedEventArgs e) {
            btnClose.IsEnabled = false;
            btnStart.IsEnabled = true;
            btnReset.IsEnabled = false;
            ws.Close();
        }

        // 重置轨迹
        private void btnReset_Click(object sender, RoutedEventArgs e) {
            pathFigure.Segments.Clear();
        }
    }
}
