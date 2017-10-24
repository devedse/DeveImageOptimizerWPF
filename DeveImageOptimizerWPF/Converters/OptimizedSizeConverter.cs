using System;
using System.Globalization;
using System.Windows.Data;
using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(OptimizedFileResult), typeof(string))]
    public sealed class OptimizedSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ofr = (OptimizedFileResult)value;
            return ValuesToStringHelper.BytesToString(ofr.OriginalSize - ofr.OptimizedSize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
