using DeveImageOptimizer.State;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    public class FilesProcessingState : INotifyPropertyChanged, IFilesProcessedListener
    {
        public ObservableCollection<OptimizedFileResult> ProcessedFiles { get; set; } = new ObservableCollection<OptimizedFileResult>();
        public OptimizedFileResult SelectedProcessedFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddProcessedFile(OptimizedFileResult optimizedFileResult)
        {
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
