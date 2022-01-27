using DeveCoolLib.ConsoleOut;
using DeveCoolLib.Streams;
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
        private int lineCount = 0;

        private LoggerExtractinator(MovingMemoryStream movingMemoryStream)
        {
            _reader = new StreamReader(movingMemoryStream);
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
                        lineCount++;

                        while (LogLines.Count > 1000)
                        {
                            LogLines.Dequeue();
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
