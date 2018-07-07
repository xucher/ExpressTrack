using ExpressTrack.DB;
using ExpressTrack.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ExpressTrack.Converters {
    [ValueConversion(typeof(int), typeof(string))]
    class ExpressStateConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string state = "";
            switch((int)value) {
                case (int)ExpressListPage.ExpressState.INIT:
                    state = "卖家已发货";break;
                case (int)ExpressListPage.ExpressState.DELIVERING:
                    state = "正在运往 " + MySqlHelper.getLastOutstock().ToStation; break;
                case (int)ExpressListPage.ExpressState.INSTOCK:
                    if (MySqlHelper.getLastInstock() != null) {
                        state = "暂存于 " + MySqlHelper.getLastInstock().ToStation;
                    }; break;
                case (int)ExpressListPage.ExpressState.FINISH:
                    state = "即将到达代收点"; break;
            }
            return state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int state = -1;
            switch((string) value) {
                case "卖家已发货":
                    state = (int)ExpressListPage.ExpressState.INIT; break;
                case "运送中":
                    state = (int)ExpressListPage.ExpressState.DELIVERING; break;
                case "在库":
                    state = (int)ExpressListPage.ExpressState.INSTOCK; break;
                case "即将到达代收点":
                    state = (int)ExpressListPage.ExpressState.FINISH; break;
            }
            return state;
        }
    }
}
