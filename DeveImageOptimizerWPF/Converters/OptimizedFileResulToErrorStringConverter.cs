using System;
using System.Globalization;
using System.Windows.Data;
using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(OptimizedFileResult), typeof(string))]
    public sealed class OptimizedFileResulToErrorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (OptimizedFileResult)value;
            return string.Join(Environment.NewLine, ofr.Errors);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
