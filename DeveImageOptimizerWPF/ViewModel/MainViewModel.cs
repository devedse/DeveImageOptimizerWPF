using DeveImageOptimizer.Exceptions;
using DeveImageOptimizer.FileProcessing;
using DeveImageOptimizer.State;
using DeveImageOptimizer.State.StoringProcessedDirectories;
using DeveImageOptimizerWPF.Helpers;
using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.MainWindowState;
using DeveImageOptimizerWPF.State.UserSettings;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeveImageOptimizerWPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ObservableRecipient
    {
        public WindowState WindowState { get; set; }
        public FileProgressState FilesProcessingState { get; set; }

        public bool PreviewEnabled { get; set; }
        public bool IsOptimizing { get; set; }

        public ICommand ShowConsoleCommand { get; }
        public ICommand HideConsoleCommand { get; }

        private readonly FileProcessedStateRememberer _fileRememberer;
        private readonly DirProcessedStateRememberer _dirRememberer;

        public MainViewModel()
        {
            WindowState = StaticState.WindowStateManager.State;
            FilesProcessingState = new FileProgressState();

            WindowState.PropertyChanged += ProcessingStateData_PropertyChanged;
            FilesProcessingState.PropertyChanged += FilesProcessingState_PropertyChanged;

            GoCommand = new AsyncRelayCommand(GoCommandImp);
            BrowseCommand = new RelayCommand(BrowseCommandImp);

            var optimize = GetRemembererSettings();

            _fileRememberer = new FileProcessedStateRememberer(optimize.fileOptimize);
            _dirRememberer = new DirProcessedStateRememberer(optimize.dirOptimize);

            StaticState.UserSettingsManager.State.PropertyChanged += State_PropertyChanged;
        }

        private (bool fileOptimize, bool dirOptimize) GetRemembererSettings()
        {
            var state = StaticState.UserSettingsManager.State;

            var fileOptimize = state.RemembererSettings == RemembererSettings.OptimizeAlways || state.RemembererSettings == RemembererSettings.StorePerDirectory;
            var dirOptimize = state.RemembererSettings == RemembererSettings.OptimizeAlways || state.RemembererSettings == RemembererSettings.StorePerFile;

            return (fileOptimize, dirOptimize);
        }

        private void State_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var optimize = GetRemembererSettings();

            _fileRememberer.ShouldAlwaysOptimize = optimize.fileOptimize;
            _dirRememberer.ShouldAlwaysOptimize = optimize.dirOptimize;
        }

        private void FilesProcessingState_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void ProcessingStateData_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StaticState.WindowStateManager.Save();
        }

        public ICommand GoCommand { get; private set; }
        private async Task GoCommandImp()
        {
            IsOptimizing = true;
            try
            {
                var state = StaticState.UserSettingsManager.State;

                var config = state.ToDeveImageOptimizerConfiguration();

                var fileProcessor = new DeveImageOptimizerProcessor(config, FilesProcessingState, _fileRememberer, _dirRememberer);

                await fileProcessor.ProcessDirectory(WindowState.ProcessingDirectory);
            }
            catch (FileOptimizerNotFoundException ex)
            {
                ShowFileOptimizerNotFoundError(ex.Message);
            }
            catch (AggregateException ex) when (ex.InnerExceptions?.OfType<FileOptimizerNotFoundException>()?.Any() == true)
            {
                var message = ex.InnerExceptions?.OfType<FileOptimizerNotFoundException>().FirstOrDefault()?.Message;
                ShowFileOptimizerNotFoundError(message);
            }
            finally
            {
                IsOptimizing = false;
            }
        }

        private static void ShowFileOptimizerNotFoundError(string? message)
        {
            System.Windows.MessageBox.Show(message, "Could not find FileOptimizer.exe", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

        public ICommand BrowseCommand { get; private set; }

        private void BrowseCommandImp()
        {
            var folderDialog = new VistaFolderBrowserDialog();

            string startDir = InitialDirFinder.FindStartingDirectoryBasedOnInput(WindowState.ProcessingDirectory);
            if (Directory.Exists(startDir))
            {
                folderDialog.SelectedPath = startDir;
            }

            if (folderDialog.ShowDialog() == true)
            {
                WindowState.ProcessingDirectory = folderDialog.SelectedPath;
            }
        }
    }
}