using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeveImageOptimizerWPF.State
{
    [AddINotifyPropertyChangedInterface]
    public class LoggerExtractinator
    {
        private readonly TextReader _textReader;

        private object _lockject = new object();
        private bool _isAlreadyRunning = false;

        public ObservableCollection<string> LogLines { get; set; } = new ObservableCollection<string>();

        public LoggerExtractinator(TextReader textReader)
        {
            _textReader = textReader;
        }

        public void GoRun()
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
                var logLine = _textReader.ReadLine();
                if (logLine != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => this.LogLines.Add(logLine)));
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
