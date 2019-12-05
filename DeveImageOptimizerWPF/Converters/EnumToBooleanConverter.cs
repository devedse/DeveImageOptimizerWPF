using System;
using System.Globalization;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null && parameter == null)
            {
                return true;
            }
            else if (value == null)
            {
                return false;
            }
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
