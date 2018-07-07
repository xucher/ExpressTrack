using System;
using System.Windows;

namespace ExpressTrack {
    public class Helpers {
        // 全局localsense连接
        public static void connGlobalLocalSense() {
            (Application.Current.MainWindow as MainWindow).startConn();
        }
        public static void closeGlobalLocalSense() {
            (Application.Current.MainWindow as MainWindow).closeConn();
        }

        public static void showMsg(string message) {
            (Application.Current.MainWindow as MainWindow).showMessage(message);
        }

        public static int[] getPostion() {
            return (Application.Current.MainWindow as MainWindow).getPosition();
        }

        // 根据定位数据获取station
        public static string getStationByPosition() {
            int[] position = getPostion();
            if (position.Length == 2) {
                if (position[0] < 100) {
                    if (position[1] > 50 && position[1] < 90) {
                        return "StationF";
                    }
                    if (position[1] > 110 && position[1] < 150) {
                        return "StationE";
                    }
                    if (position[1] > 170 && position[1] < 210) {
                        return "StationD";
                    }
                    if (position[1] > 230 && position[1] < 270) {
                        return "StationC";
                    }
                } else if (position[0] > 300) {
                    if (position[1] > 50 && position[1] < 90) {
                        return "StationA";
                    }
                    if (position[1] > 500 && position[1] < 540) {
                        return "StationB";
                    }
                }
            }
            return null;
        }
        // 判断是否即将到达StationB
        public static bool isApproaching(int range) {
            int[] position = getPostion();
            if (position[0] > 300-range && 
                position[1] > 500-range && position[1] < 540+range) {
                return true;
            }
            return false;
        }

        // 解析快递编号  pattern: 2018 11 25 0000
        public static int parseExpressCoding(string coding) {
            return int.Parse(coding.Substring(8));
        }
        // 将序号转换成编号
        public static string convertExpressCoding(int index) {
            var now = DateTime.Now;
            return (now.Year+"") + (now.Month+"").PadLeft(2, '0') + 
                (now.Day+"").PadLeft(2, '0') + (index + "").PadLeft(4, '0');
        }

        public static int parseShipmentCoding(string coding) {
            return int.Parse(coding.Substring(4));
        }

        public static string convertShipmentCoding(int index, int type) {
            if (type == ShipmentPage.INSTOCK) {
                return "I000" + (index + "").PadLeft(4, '0');
            } else {
                return "O000" + (index + "").PadLeft(4, '0');
            }
        }

        // 解析预定路线
        public static string[] parsePreTrack(string preTrack) {
            return preTrack.Split('-');
        }

        // 解析localsense数据
        public static int[] parseLocalsenseBlob(string data) {
            // Blob example:  CC-5F-01-01-3A-41-00-00-01-6C-00-00-02-E1-00-AE-01-02-00-03-58-F2-AC-01-01-27-07-AA-BB
            //location:  id(设备编号), x, y
            int[] location = new int[3];
            string[] tempdata = data.Split('-');
            int msgtype = -1;

            if (tempdata[0] == "CC" && tempdata[1] == "5F" && tempdata[2] == "01") {
                msgtype = 1;//表示标签实时信息

                int tagnum = Convert.ToInt16(tempdata[3], 16);
                for (int i = 0; i < tagnum; i++) {
                    location[0] = Convert.ToInt16(tempdata[4 + 21 * (tagnum - 1)] + tempdata[5 + 21 * (tagnum - 1)], 16);
                    location[1] = Convert.ToInt16(tempdata[6 + 21 * (tagnum - 1)] + tempdata[7 + 21 * (tagnum - 1)] + tempdata[8 + 21 * (tagnum - 1)] + tempdata[9 + 21 * (tagnum - 1)], 16);
                    location[2] = Convert.ToInt16(tempdata[10 + 21 * (tagnum - 1)] + tempdata[11 + 21 * (tagnum - 1)] + tempdata[12 + 21 * (tagnum - 1)] + tempdata[13 + 21 * (tagnum - 1)], 16);
                }
            }
            return location;
        }
    }
}
