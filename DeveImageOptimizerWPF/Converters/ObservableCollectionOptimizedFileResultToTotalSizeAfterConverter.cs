using System;
using System.Globalization;
using System.Windows.Data;
using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using System.Linq;
using System.Collections.ObjectModel;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(ObservableCollection<OptimizedFileResult>), typeof(string))]
    public sealed class ObservableCollectionOptimizedFileResultToTotalSizeAfterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (ObservableCollection<OptimizedFileResult>)value;
            var totalOptimizedSize = ofr.Sum(t => t.OptimizedSize);
            return ValuesToStringHelper.BytesToString(totalOptimizedSize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
