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
    public class FilesProcessingState : INotifyPropertyChanged, IFilesProcessingState
    {
        public ObservableCollection<string> ProcessedFiles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> FailedFiles { get; set; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddProcessedFile(OptimizedFileResult optimizedFileResult)
        {
            if (optimizedFileResult.Successful)
            {
                ProcessedFiles.Add(optimizedFileResult.Path);
                OnPropertyChanged(nameof(ProcessedFiles));
            }
            else
            {
                FailedFiles.Add(optimizedFileResult.Path);
                OnPropertyChanged(nameof(FailedFiles));
            }
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
