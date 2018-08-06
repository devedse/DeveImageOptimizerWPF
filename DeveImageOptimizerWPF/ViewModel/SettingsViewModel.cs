using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PropertyChanged;
using System.IO;
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
            BrowseCommand = new RelayCommand(() => BrowseCommandImp(), () => true);

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

        public ICommand BrowseCommand { get; private set; }
        private void BrowseCommandImp()
        {
            var startDir = UserSettingsData.FileOptimizerPath;

            while (!Directory.Exists(startDir) && !string.IsNullOrWhiteSpace(startDir))
            {
                startDir = Path.GetDirectoryName(startDir);
            }

            var fileDialog = new OpenFileDialog()
            {
                Filter = "FileOptimizer (FileOptimizer.exe,FileOptimizer64.exe)|FileOptimizer.exe;FileOptimizer64.exe|All files (*.*)|*.*"
            };

            if (Directory.Exists(startDir))
            {
                fileDialog.InitialDirectory = startDir;
            }

            if (fileDialog.ShowDialog() == true)
            {
                UserSettingsData.FileOptimizerPath = fileDialog.FileName;
            }
        }
    }
}
