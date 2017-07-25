using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeveImageOptimizerWPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsViewModel : ViewModelBase
    {
        public UserSettingsData UserSettingsData { get; private set; }

        public SettingsViewModel()
        {
            UserSettingsData = StaticState.UserSettingsManager.State;
            SaveCommand = new RelayCommand(() => SaveCommandImp(), () => true);
        }

        public ICommand SaveCommand { get; private set; }
        private void SaveCommandImp()
        {
            StaticState.UserSettingsManager.Save();
        }
    }
}
