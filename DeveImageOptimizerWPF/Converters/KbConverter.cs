using DeveCoolLib.Conversion;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public sealed class KbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValuesToStringHelper.BytesToString((long)value, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
