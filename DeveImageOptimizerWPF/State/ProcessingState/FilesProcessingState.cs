using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    public class FilesProcessingState : INotifyPropertyChanged, IFilesProcessedListener
    {
        private readonly string _logPath;

        public ObservableCollection<OptimizableFile> ProcessedFiles { get; set; } = new ObservableCollection<OptimizableFile>();
        public OptimizableFile SelectedProcessedFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private object logfilelockject = new object();

        public FilesProcessingState()
        {
            _logPath = Path.Combine(FolderHelperMethods.ConfigFolder, "Log.txt");
        }

        // Create the OnPropertyChanged method to raise the event
        public virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddProcessedFile(OptimizableFile optimizedFileResult)
        {
            if (optimizedFileResult.OptimizationResult == OptimizationResult.InProgress)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ProcessedFiles.Add(optimizedFileResult);
                }));

                //ProcessedFiles.Add(optimizedFileResult);
            }
            else
            {
                lock (logfilelockject)
                {
                    File.AppendAllText(_logPath, $"{optimizedFileResult.OptimizationResult}|{optimizedFileResult.Path}{Environment.NewLine}");
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var iii = ProcessedFiles.IndexOf(optimizedFileResult);

                    ProcessedFiles.RemoveAt(iii);
                    ProcessedFiles.Insert(iii, optimizedFileResult);
                }));
            }



            OnPropertyChanged(nameof(ProcessedFiles));
        }
    }
}
