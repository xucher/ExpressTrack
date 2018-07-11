using ExpressTrack.DB;
using ExpressTrack.Models;
using ExpressTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WebSocketSharp;
using System.Linq;

namespace ExpressTrack {
    public partial class ExpressDetailPage : Page {
        public ExpressDetailPage() {
            InitializeComponent();

            inputCoding.ItemsSource = from e in MySqlHelper.getAllExpress()
                                      select e.Coding;

            DataContext = new ExpressDetailViewModel {
                NowAddress = "未连接设备...",
                PreTrack = new ObservableCollection<string>(),
                ShipRecords = new ObservableCollection<ExpressDetailViewModel.ShipRecord>(),
                ExpressState = -1
            };
            mViewModel = DataContext as ExpressDetailViewModel;
        }
        private ExpressDetailViewModel mViewModel;
        private PathFigure pathFigure;
        private WebSocket ws;
        private Stopwatch timer;
        private int flag = 1;   // 标识是否为第一个数据

        // 处理获得的坐标数据
        private void handleLocalSense(object sender, MessageEventArgs e) { 
            //  maxX=1000, maxY = 1375
            if (!timer.IsRunning) {
                timer.Start();
            }
            
            if (timer.ElapsedMilliseconds > 50) {
                int[] location = Helpers.parseLocalsenseBlob(e.Data);
                if (location[0] == 15027) {
                    int x = (int)(location[1] / 2.5);
                    int y = (int)(550 - location[2] / 2.5);
                    
                    if (flag == 1) {
                        Dispatcher.Invoke(new Action(delegate {
                            pathFigure = new PathFigure { StartPoint = new Point(x, y) };
                            trackGeometry.Figures.Add(pathFigure);
                        }));
                        flag = 0;
                    } else {
                        Dispatcher.Invoke(new Action(delegate {
                            if (pathFigure != null) {
                                pathFigure.Segments.Add(new LineSegment(new Point(x, y), true));
                            }
                        }));
                    }
                    Dispatcher.Invoke(new Action(delegate {
                        (DataContext as ExpressDetailViewModel).NowAddress =
                                "(" + x + ", " + y + ")";
                    }));
                }

                timer.Reset();
                timer.Start();
            }
        }

        // 开始追踪
        private void btnStart_Click(object sender, RoutedEventArgs e) {
            if (ws == null) {
                initWebsocket();
            }

            if (timer == null) {
                timer = new Stopwatch();
            }
            timer.Reset();

            ws.Connect();
            if (ws.ReadyState == WebSocketState.Closed) {
                Helpers.showMsg("连接失败");
            }
        }

        // 初始化websocket
        private void initWebsocket() {
            ws = new WebSocket(url: "ws://192.168.0.151:9001", protocols: "localSensePush-protocol");
            
            ws.OnOpen += (sender, e) => {
                btnStart.IsEnabled = false;
                btnClose.IsEnabled = true;
                btnReset.IsEnabled = true;
                Helpers.showMsg("已成功连接Localsense");
            };
            ws.OnMessage += handleLocalSense;
            ws.OnClose += (sender, e) => {
                btnStart.IsEnabled = true;
                btnClose.IsEnabled = false;
                btnReset.IsEnabled = false;
                Helpers.showMsg("已断开连接");
            };
            ws.OnError += (sender, e) => {
                Helpers.showMsg("Websocket异常");
                Console.WriteLine("Websocket异常，错误信息为" + e.Message);
            };
        }

        // 关闭连接
        private void btnClose_Click(object sender, RoutedEventArgs e) {
            ws.Close();
        }

        // 重置轨迹
        private void btnReset_Click(object sender, RoutedEventArgs e) {
            pathFigure.Segments.Clear();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e) {
            Express express = MySqlHelper.getExpressByCoding(inputCoding.Text);
            if (express != null) {
                mViewModel.ExpressName = express.Name;
                mViewModel.ExpressState = express.State;
                mViewModel.PreTrack.Clear();

                if (express.PreTrack != null) {
                    foreach (var item in Helpers.parsePreTrack(express.PreTrack)) {
                        mViewModel.PreTrack.Add(item);
                    }
                } else {
                    Helpers.showMsg("未设置预定路线");
                }
                mViewModel.ShipRecords.Clear();
                foreach (var record in MySqlHelper.getShipRecord(inputCoding.Text)) {
                    mViewModel.ShipRecords.Add(record);
                }
                setTimeLine();
            } else {
                Helpers.showMsg("该编号的快递不存在");
            }
        }

        private void setTimeLine() {
            LTimeLine.Visibility = Visibility.Visible;
            if (mViewModel.ShipRecords.Count == 0) {
                LTimeLine.Y2 = 20;
            } else {
                LTimeLine.Y2 = (mViewModel.ShipRecords.Count - 1) * 52 + 20;
            }
            LTimeLine.StrokeDashArray = new DoubleCollection { 5,1 };
        }
    }
}
