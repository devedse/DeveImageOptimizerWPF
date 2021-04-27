﻿using DeveCoolLib.Conversion;
using DeveImageOptimizerWPF.State.ProcessingState;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(ObservableCollection<OptimizableFileUI>), typeof(string))]
    public sealed class ObservableCollectionOptimizedFileResultToTotalSizeAfterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (ObservableCollection<OptimizableFileUI>)value;
            var totalOptimizedSize = ofr.Sum(t => t.OptimizedSize);
            return ValuesToStringHelper.BytesToString(totalOptimizedSize, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
