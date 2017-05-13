using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EpxViewer
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class Bool2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool sourceValue = System.Convert.ToBoolean(value);
            if (sourceValue) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility sourceValue = (Visibility)value;
            if (sourceValue == Visibility.Visible) return true;
            else return false;
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class Bool2NotBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool sourceValue = System.Convert.ToBoolean(value);
            return !sourceValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool sourceValue = System.Convert.ToBoolean(value);
            return !sourceValue;
        }
    }
}
