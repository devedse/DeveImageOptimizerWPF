﻿using DeveImageOptimizerWPF.State.ProcessingState;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.Converters
{
    [ValueConversion(typeof(OptimizableFileUI), typeof(string))]
    public sealed class OptimizedFileResulToErrorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var ofr = (OptimizableFileUI)value;
            return string.Join(Environment.NewLine, ofr.Errors);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
