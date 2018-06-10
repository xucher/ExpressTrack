using System;
using System.Globalization;
using System.Windows.Data;

namespace ExpressTrack.Converters {
    [ValueConversion(typeof(int), typeof(string))]
    class ExpressStateConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string state = "";
            switch((int)value) {
                case 0:
                    state = "初始";break;
                case 1:
                    state = "运送中"; break;
                case 2:
                    state = "在库"; break;
                case 3:
                    state = "完成"; break;
            }
            return state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int state = -1;
            switch((string) value) {
                case "初始":
                    state = 0; break;
                case "运送中":
                    state = 1; break;
                case "在库":
                    state = 2; break;
                case "完成":
                    state = 3; break;
            }
            return state;
        }
    }
}
