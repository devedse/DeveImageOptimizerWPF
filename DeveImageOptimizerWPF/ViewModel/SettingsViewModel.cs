using DeveImageOptimizerWPF.Helpers;
using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace DeveImageOptimizerWPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsViewModel : ViewModelBase
    {
        public UserSettingsData UserSettingsData { get; }

        public IEnumerable<int> MaxParallelismChoices { get; }
        public IEnumerable<int> AvailableLogLevels { get; }

        public IEnumerable<RemembererSettings> AvailableStorageModes { get; }

        public SettingsViewModel()
        {
            UserSettingsData = StaticState.UserSettingsManager.State;
            BrowseCommand = new RelayCommand(() => BrowseCommandImp(), () => true);

            SaveCommand = new RelayCommand(SaveCommandImp, () => true);
            ResetToDefaultsCommand = new RelayCommand(ResetToDefaultsCommandImpl, () => true);

            MaxParallelismChoices = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            AvailableLogLevels = new List<int>() { 0, 1, 2, 3, 4 };

            AvailableStorageModes = Enum.GetValues(typeof(RemembererSettings)).Cast<RemembererSettings>().ToList();
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
            var fileDialog = new OpenFileDialog()
            {
                Filter = "FileOptimizer (FileOptimizer.exe,FileOptimizer64.exe)|FileOptimizer.exe;FileOptimizer64.exe|All files (*.*)|*.*"
            };

            string startDir = InitialDirFinder.FindStartingDirectoryBasedOnInput(UserSettingsData.FileOptimizerPath);
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
