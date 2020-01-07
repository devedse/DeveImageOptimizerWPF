using DeveImageOptimizerWPF.Helpers;
using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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
            BrowseCommandFileOptimizer = new RelayCommand(() => BrowseCommandFileOptimizerImp(), () => true);
            BrowseCommandTempDir = new RelayCommand(() => BrowseCommandTempDirImp(), () => true);

            SaveCommand = new RelayCommand(SaveCommandImp, () => true);
            ResetToDefaultsCommand = new RelayCommand(ResetToDefaultsCommandImpl, () => true);

            MaxParallelismChoices = Enumerable.Range(1, Environment.ProcessorCount).ToList();
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

        public ICommand BrowseCommandFileOptimizer { get; private set; }
        private void BrowseCommandFileOptimizerImp()
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

        public ICommand BrowseCommandTempDir { get; private set; }

        private void BrowseCommandTempDirImp()
        {
            var folderDialog = new VistaFolderBrowserDialog();

            string startDir = InitialDirFinder.FindStartingDirectoryBasedOnInput(UserSettingsData.TempDirectory);
            if (Directory.Exists(startDir))
            {
                folderDialog.SelectedPath = startDir;
            }

            if (folderDialog.ShowDialog() == true)
            {
                UserSettingsData.TempDirectory = folderDialog.SelectedPath;
            }
        }
    }
}
