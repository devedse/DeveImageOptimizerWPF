using DeveCoolLib.ConsoleOut;
using DeveCoolLib.Streams;
using DeveImageOptimizerWPF.LogViewerData;
using IX.Observable;
using PropertyChanged;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeveImageOptimizerWPF.ViewModel.ObservableData
{
    [AddINotifyPropertyChangedInterface]
    public class LoggerExtractinator
    {
        public static LoggerExtractinator CreateLoggerExtractinatorAndSetupConsoleRedirection()
        {
            var originalOut = Console.OpenStandardOutput();
            var consoleOutputStream = new MovingMemoryStream();

            var multiOut = new MultiStream(originalOut, consoleOutputStream);
            //var multiOut = new MultiStream(originalOut);
            var writer = new StreamWriter(multiOut)
            {
                AutoFlush = true
            };

            Console.SetOut(writer);

            var extractinator = new LoggerExtractinator(consoleOutputStream);
            extractinator.GoRun();
            return extractinator;
        }

        private readonly TextReader _reader;
        private object _lockject = new object();
        private bool _isAlreadyRunning = false;

        public ObservableQueue<string> LogLines { get; set; } = new ObservableQueue<string>();
        public ObservableQueue<LogEntry> LogLinesEntry { get; set; } = new ObservableQueue<LogEntry>();
        private int lineCount = 0;

        private LoggerExtractinator(MovingMemoryStream movingMemoryStream)
        {
            _reader = new StreamReader(movingMemoryStream);

            Console.WriteLine("Creating logreader 1 :)");
            Console.WriteLine("Creating logreader 2 :)");
            Console.WriteLine("Creating logreader 3 :)");
            Console.WriteLine("Creating logreader 4 :)");
        }

        private void GoRun()
        {
            lock (_lockject)
            {
                if (_isAlreadyRunning)
                {
                    return;
                }
                _isAlreadyRunning = true;
            }

            Task.Run(Runner);
        }

        private void Runner()
        {
            while (true)
            {
                var logLine = _reader.ReadLine();
                if (logLine != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var lineToAdd = $"{lineCount,4}: {logLine}";
                        LogLines.Enqueue(lineToAdd);
                        LogLinesEntry.Enqueue(new LogEntry() { DateTime = DateTime.Now, Index = lineCount, Message = logLine });
                        lineCount++;

                        while (LogLines.Count > 1000)
                        {
                            LogLines.Dequeue();
                        }
                        while (LogLinesEntry.Count > 1000)
                        {
                            LogLinesEntry.Dequeue();
                        }
                    }));
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
