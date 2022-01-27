using DeveImageOptimizerWPF.LogViewerData;
using IX.Observable;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DeveImageOptimizerWPF
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
        public static readonly DependencyProperty LogLinesProperty = DependencyProperty.Register(
            "LogLines",
            typeof(ObservableQueue<LogEntry>),
            typeof(LogViewer),
            new PropertyMetadata(new ObservableQueue<LogEntry>(new List<LogEntry>() { new LogEntry() { DateTime = DateTime.Now, Index = 0, Message = "Test" } })));

        public ObservableQueue<LogEntry> LogLines
        {
            get
            {
                return (ObservableQueue<LogEntry>)GetValue(LogLinesProperty);
            }
            set
            {
                SetValue(LogLinesProperty, value);
            }
        }

        public static readonly DependencyProperty LogViewerFontSizeProperty = DependencyProperty.Register(
                    "LogViewerFontSize",
                    typeof(int),
                    typeof(LogViewer),
                    new PropertyMetadata(12));

        public int LogViewerFontSize
        {
            get
            {
                return (int)GetValue(LogViewerFontSizeProperty);
            }
            set
            {
                SetValue(LogViewerFontSizeProperty, value);
            }
        }

        public LogViewer()
        {
            InitializeComponent();
        }
    }
}
