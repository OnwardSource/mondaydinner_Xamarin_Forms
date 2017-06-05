using System;
using System.Globalization;
using Xamarin.Forms;

namespace mondaydinner
{
    public class BooleanToObjectConverter : IValueConverter
    {
        public object TrueValue { get; set; }

        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return null;

            var boolValue = (bool)value;

            return boolValue ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue == null) return false;

            return stringValue.Equals(TrueValue);
        }
    }
}
