using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DeveImageOptimizer.State;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public sealed class ToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (bool)value;
            if (ofr)
            {
                return Brushes.LightGreen;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
