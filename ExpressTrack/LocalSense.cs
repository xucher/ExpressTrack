//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WebSocketSharp;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExpressTrack.Device {
//    class LocalSense {
//        public delegate void ConnectHandle(WebSocketHandle w, EventArgs e);
//        public event ConnectHandle OnConnect;
//        public int[] location = null
//            ;
//        public int[] connect_server() {
//            //连接服务器
//            WebSocket ws = new WebSocket("ws://" + "192.168.0.151" + ":9001", "localSensePush-protocol");

//            int id;
//            int x;
//            int y;

//            ws.OnOpen += (sender, e) => MessageBox.Show("已经与服务器建立连接");
//            ws.OnMessage += (sender, e) => {
//                string data = e.Data;
//                string[] tempdata = data.Split('-');
//                int msgtype;

//                if (tempdata[0] == "CC" && tempdata[1] == "5F" && tempdata[2] == "01") {
//                    msgtype = 1;//表示标签实时信息

//                    int tagnum = Convert.ToInt16(tempdata[3], 16);
//                    for (int i = 0; i < tagnum; i++) {

//                        id = Convert.ToInt16(tempdata[4 + 21 * (tagnum - 1)] + tempdata[5 + 21 * (tagnum - 1)], 16);
//                        x = Convert.ToInt16(tempdata[6 + 21 * (tagnum - 1)] + tempdata[7 + 21 * (tagnum - 1)] + tempdata[8 + 21 * (tagnum - 1)] + tempdata[9 + 21 * (tagnum - 1)], 16);
//                        y = Convert.ToInt16(tempdata[10 + 21 * (tagnum - 1)] + tempdata[11 + 21 * (tagnum - 1)] + tempdata[12 + 21 * (tagnum - 1)] + tempdata[13 + 21 * (tagnum - 1)], 16);
//                        if (location == null) {
//                            location = new int[3];
//                        }
//                        location[0] = id;
//                        location[1] = x;
//                        location[2] = y;

//                        if (OnConnect != null) {
//                            OnConnect(this, e);
//                        }
//                    }
//                }

//            };
//            ws.Connect();
//            ws.OnClose += (sender, e) => {
//                Console.WriteLine("已经和服务器断开，关闭状态为：" + e.Code + "关闭原因是：" + e.Reason);
//            };
//            ws.OnError += (sender, e) => {
//                Console.WriteLine("Websocket异常，错误信息为" + e.Message);
//            };
//            return location;
//        }
//    }
//}
