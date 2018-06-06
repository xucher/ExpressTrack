using System;
using System.Globalization;
using System.Windows.Data;

namespace ExpressTrack.Converters {
    [ValueConversion(typeof(bool), typeof(char))]
    public class BoolToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((bool) value) {
                return 'Y';
            } else {
                return 'N';
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
           if ((char) value == 'Y') {
                return true;
           } else {
                return false;
            }
        }
    }
}
