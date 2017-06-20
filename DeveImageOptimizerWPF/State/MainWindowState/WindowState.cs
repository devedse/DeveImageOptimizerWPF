using System;
using System.ComponentModel;

namespace DeveImageOptimizerWPF.State.MainWindowState
{
    [Serializable]
    public class WindowState : INotifyPropertyChanged
    {
        public string ProcessingDirectory { get; set; } = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
