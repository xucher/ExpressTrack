using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressTrack {
    public class Helpers {
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

        // 解析localsense数据
        public static int[] parseLocalsenseBlob(string data) {
            //location:  id(设备编号), x, y
            int[] location = new int[3];
            string[] tempdata = data.Split('-');
            int msgtype;

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
