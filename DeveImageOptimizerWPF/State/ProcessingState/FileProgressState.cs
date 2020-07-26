using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using DeveImageOptimizerWPF.Helpers;
using DeveImageOptimizerWPF.State.ProcessingState;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    public class FileProgressState : INotifyPropertyChanged, IProgressReporter
    {
        private readonly string _logPath;

        public AutoFilteringObservableCollection<OptimizableFileUI> ProcessedFiles { get; set; } = new AutoFilteringObservableCollection<OptimizableFileUI>();
        public OptimizableFileUI SelectedProcessedFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object _logfilelockject = new object();

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

        public Task OptimizableFileProgressUpdated(OptimizableFile optimizableFile)
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

                //This code is required to have an item that updates from Processing to Skipped be re-filtered
                var indexOf = ProcessedFiles.IndexOf(foundFile);

                var eventArgs = new NotifyCollectionChangedEventArgs(
                                        NotifyCollectionChangedAction.Replace,
                                        new List<object> { foundFile },
                                        new List<object> { foundFile }, indexOf);

                ProcessedFiles.RaiseCollectionChanged(eventArgs);
            }

            OnPropertyChanged(nameof(ProcessedFiles));

            return Task.CompletedTask;
        }

        public Task TotalFileCountDiscovered(int count)
        {
            //We don't need this yet
            return Task.CompletedTask;
        }
    }
}
