using DeveImageOptimizerWPF.LogViewerData;
using IX.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DeveImageOptimizerWPF
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
        private string TestData = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        private List<string> words;
        private int maxword;
        private int index;

        //public ObservableCollection<LogEntry> LogEntries { get; set; }

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

            //this.DataContext = this;

            random = new Random();
            words = TestData.Split(' ').ToList();
            maxword = words.Count - 1;

            //LogEntries = new ObservableQueue<LogEntry>();
            //Enumerable.Range(0, 200)
            //          .ToList()
            //          .ForEach(x => LogEntries.Add(GetRandomEntry()));

            //Timer = new Timer(x => AddRandomEntry(), null, 1000, 10);
        }

        private System.Threading.Timer Timer;
        private System.Random random;
        //private void AddRandomEntry()
        //{
        //    Dispatcher.BeginInvoke(() =>
        //    {
        //        LogEntries.Enqueue(GetRandomEntry());

        //        while (LogEntries.Count > 1000)
        //        {
        //            LogEntries.Dequeue();
        //        }
        //    });
        //}

        private LogEntry GetRandomEntry()
        {
            if (random.Next(1, 10) > 0)
            {
                return new LogEntry
                {
                    Index = index++,
                    DateTime = DateTime.Now,
                    Message = string.Join(" ", Enumerable.Range(5, random.Next(10, 50))
                                                         .Select(x => words[random.Next(0, maxword)])),
                };
            }

            return new CollapsibleLogEntry
            {
                Index = index++,
                DateTime = DateTime.Now,
                Message = string.Join(" ", Enumerable.Range(5, random.Next(10, 50))
                                                     .Select(x => words[random.Next(0, maxword)])),
                Contents = Enumerable.Range(5, random.Next(5, 10))
                                     .Select(i => GetRandomEntry())
                                     .ToList()
            };
        }
    }
}
