using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System.Windows.Input;

namespace DeveImageOptimizerWPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsViewModel : ViewModelBase
    {
        public UserSettingsData UserSettingsData { get; }

        public SettingsViewModel()
        {
            UserSettingsData = StaticState.UserSettingsManager.State;
            SaveCommand = new RelayCommand(SaveCommandImp, () => true);
            ResetToDefaultsCommand = new RelayCommand(ResetToDefaultsCommandImpl, () => true);
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandImp()
        {
            StaticState.UserSettingsManager.Save();
        }

        public ICommand ResetToDefaultsCommand { get; }
        private void ResetToDefaultsCommandImpl()
        {
            StaticState.UserSettingsManager.State.ResetToDefaults();
        }
    }
}
