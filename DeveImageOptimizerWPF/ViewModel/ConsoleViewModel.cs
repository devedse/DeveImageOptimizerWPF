using DeveImageOptimizerWPF.ViewModel.ObservableData;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PropertyChanged;
using System;
using System.Windows.Input;

namespace DeveImageOptimizerWPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel : ObservableRecipient
    {
        public LoggerExtractinator LoggerExtractinator { get; set; }

        public int ConsoleFontSize { get; set; } = 12;
        public ICommand IncreaseFontSizeCommand { get; }
        public ICommand DecreaseFontSizeCommand { get; }

        public ConsoleViewModel(LoggerExtractinator loggerExtractinator)
        {
            LoggerExtractinator = loggerExtractinator;

            IncreaseFontSizeCommand = new RelayCommand(() => ConsoleFontSize = Math.Clamp(ConsoleFontSize + 2, 12, 30));
            DecreaseFontSizeCommand = new RelayCommand(() => ConsoleFontSize = Math.Clamp(ConsoleFontSize - 2, 12, 30));
        }
    }
}
