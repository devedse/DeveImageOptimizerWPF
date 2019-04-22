using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    public class FilesProcessingState : INotifyPropertyChanged, IFilesProcessedListener
    {
        private readonly string _logPath;

        public ObservableCollection<OptimizedFileResult> ProcessedFiles { get; set; } = new ObservableCollection<OptimizedFileResult>();
        public OptimizedFileResult SelectedProcessedFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FilesProcessingState()
        {
            _logPath = Path.Combine(FolderHelperMethods.EntryAssemblyDirectory.Value, "Log.txt");
        }

        public void AddProcessedFile(OptimizedFileResult optimizedFileResult)
        {
            File.AppendAllText(_logPath, $"{optimizedFileResult.OptimizationResult}|{optimizedFileResult.Path}{Environment.NewLine}");
            ProcessedFiles.Add(optimizedFileResult);
            OnPropertyChanged(nameof(ProcessedFiles));
        }

        // Create the OnPropertyChanged method to raise the event
        public virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
