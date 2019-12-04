using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using DeveImageOptimizerWPF.State.ProcessingState;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    public class FileProgressState : INotifyPropertyChanged, IProgressReporter
    {
        private readonly string _logPath;

        public ObservableCollection<OptimizableFileUI> ProcessedFiles { get; set; } = new ObservableCollection<OptimizableFileUI>();
        public OptimizableFileUI SelectedProcessedFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object logfilelockject = new object();

        public OptimizationResult? _filter { get; set; }

        public OptimizationResult? Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                ProcessedFilesView.Refresh();
            }
        }


        internal CollectionViewSource ProcessedFilesViewSource { get; set; } = new CollectionViewSource();
        public ICollectionView ProcessedFilesView => ProcessedFilesViewSource.View;


        public FileProgressState()
        {
            ProcessedFilesViewSource.Source = ProcessedFiles;
            ProcessedFilesViewSource.Filter += ApplyFilter;

            _logPath = Path.Combine(FolderHelperMethods.ConfigFolder, "Log.txt");
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            OptimizableFileUI fileUI = (OptimizableFileUI)e.Item;

            if (_filter == null || fileUI.OptimizationResult == OptimizationResult.InProgress || fileUI.OptimizationResult == _filter.Value)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        // Create the OnPropertyChanged method to raise the event
        public virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OptimizableFileProgressUpdated(OptimizableFile optimizableFile)
        {
            if (optimizableFile.OptimizationResult != OptimizationResult.InProgress)
            {
                //No need for lock because this happens on the UI thread
                File.AppendAllText(_logPath, $"{optimizableFile.OptimizationResult}|{optimizableFile.Path}{Environment.NewLine}");
            }

            var foundFile = ProcessedFiles.FirstOrDefault(t => t.Path == optimizableFile.Path);
            if (foundFile == null)
            {
                ProcessedFiles.Add(new OptimizableFileUI(optimizableFile));
            }
            else
            {
                foundFile.Set(optimizableFile);
            }

            OnPropertyChanged(nameof(ProcessedFiles));
        }
    }
}
