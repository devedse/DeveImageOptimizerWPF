using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(OptimizableFile), typeof(string))]
    public sealed class OptimizedFileResultToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (OptimizableFile)value;
            return ValuesToStringHelper.BytesToString(ofr.OriginalSize - ofr.OptimizedSize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
