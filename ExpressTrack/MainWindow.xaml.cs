using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WebSocketSharp;

namespace ExpressTrack {
    public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

            Snackbar.MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue();

            initWebSocket();
		}

        private WebSocket ws;
        private int x;
        private int y;
        private Stopwatch timer;

        public void showMessage(string message) {
            Snackbar.MessageQueue.Enqueue(message);
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				DragMove();
			}
		}

        // 初始化全局localsense连接，用于定位站点
        private void initWebSocket() {
            
            ws = new WebSocket(url: "ws://192.168.0.151:9001", protocols: "localSensePush-protocol");

            ws.OnOpen += (sender, e) => {
                Helpers.showMsg("*已成功连接Localsense");
            };
            ws.OnMessage += readPosition;
            ws.OnClose += (sender, e) => {
                Helpers.showMsg("*已断开连接");
                
                timer = null;
            };
            ws.OnError += (sender, e) => {
                Helpers.showMsg("*Websocket异常");
                Console.WriteLine("Websocket异常，错误信息为" + e.Message);
                timer = null;
            };
        }

        public void startConn() {
            ws.Connect();
        }
        public void closeConn() {
            ws.Close();
        }

        private void readPosition(object sender, MessageEventArgs e) {
            if (timer == null) {
                timer = new Stopwatch();
                timer.Start();
            } else if (timer.ElapsedMilliseconds > 1500) {
                int[] location = Helpers.parseLocalsenseBlob(e.Data);
                if (location[0] == 15027) {
                    x = (int)(location[1] / 2.5);
                    y = (int)(550 - location[2] / 2.5);
                }
                timer.Reset();
                timer.Start();
            }
        }

        public int[] getPosition() {
            return new int[] { x, y };
        }

        private void windowFrame_Unloaded(object sender, RoutedEventArgs e) {
            if (ws.ReadyState == WebSocketState.Open) {
                closeConn();
                ws = null;
            }
        }
    }
}
