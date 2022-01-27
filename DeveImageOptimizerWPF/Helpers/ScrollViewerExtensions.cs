﻿using PropertyChanged;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DeveImageOptimizerWPF.Helpers
{
    public class ScrollViewerExtensions
    {
        public static readonly DependencyProperty AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached(
            "AlwaysScrollToEnd",
            typeof(bool),
            typeof(ScrollViewerExtensions),
            new PropertyMetadata(false, AlwaysScrollToEndChanged));
        private static bool _autoScroll;

        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ScrollViewer scroll)
            {
                bool alwaysScrollToEnd = (e.NewValue != null) && (bool)e.NewValue;
                if (alwaysScrollToEnd)
                {
                    scroll.ScrollToEnd();
                    scroll.ScrollChanged += ScrollChanged;
                }
                else
                {
                    scroll.ScrollChanged -= ScrollChanged;
                }
            }
            else
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }
        }

        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll)
        {
            if (scroll == null) { throw new ArgumentNullException("scroll"); }
            return (bool)scroll.GetValue(AlwaysScrollToEndProperty);
        }

        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd)
        {
            if (scroll == null) { throw new ArgumentNullException("scroll"); }
            scroll.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }

        private static void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scroll)
            {
                // User scroll event : set or unset autoscroll mode
                if (e.ExtentHeightChange == 0)
                {
                    _autoScroll = scroll.VerticalOffset == scroll.ScrollableHeight;
                    scroll.SetValue(AlwaysScrollToEndProperty, _autoScroll);
                }

                // Content scroll event : autoscroll eventually
                if (_autoScroll && e.ExtentHeightChange != 0)
                {
                    scroll.ScrollToVerticalOffset(scroll.ExtentHeight);
                }
            }
            else
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }
        }
    }
}
