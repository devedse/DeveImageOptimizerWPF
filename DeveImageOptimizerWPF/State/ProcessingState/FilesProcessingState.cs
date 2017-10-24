using DeveImageOptimizer.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
