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
    }
}
