using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LoaSelfi.Converters;
internal class BoolToVisibleInverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool result = false;
        if(value == null) return Visibility.Visible;

        if(value is bool)
        {
            result = (bool)value;
        }

        return !result ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
