using DeveImageOptimizer.FileProcessing;
using DeveImageOptimizer.State;
using DeveImageOptimizer.State.StoringProcessedDirectories;
using DeveImageOptimizerWPF.Helpers;
using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.MainWindowState;
using DeveImageOptimizerWPF.State.UserSettings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using System.IO;
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
    public class MainViewModel : ViewModelBase
    {
        public WindowState WindowState { get; set; }
        public FileProgressState FilesProcessingState { get; set; }

        public bool PreviewEnabled { get; set; }

        private readonly FileProcessedStateRememberer fileRememberer;
        private readonly DirProcessedStateRememberer dirRememberer;

        public MainViewModel()
        {
            WindowState = StaticState.WindowStateManager.State;
            FilesProcessingState = new FileProgressState();

            WindowState.PropertyChanged += ProcessingStateData_PropertyChanged;
            FilesProcessingState.PropertyChanged += FilesProcessingState_PropertyChanged;

            GoCommand = new RelayCommand(async () => await GoCommandImp(), () => true);
            BrowseCommand = new RelayCommand(() => BrowseCommandImp(), () => true);

            var optimize = GetRemembererSettings();

            fileRememberer = new FileProcessedStateRememberer(optimize.fileOptimize);
            dirRememberer = new DirProcessedStateRememberer(optimize.dirOptimize);

            StaticState.UserSettingsManager.State.PropertyChanged += State_PropertyChanged;
        }

        private (bool fileOptimize, bool dirOptimize) GetRemembererSettings()
        {
            var state = StaticState.UserSettingsManager.State;

            var fileOptimize = state.RemembererSettings == RemembererSettings.OptimizeAlways || state.RemembererSettings == RemembererSettings.StorePerDirectory;
            var dirOptimize = state.RemembererSettings == RemembererSettings.OptimizeAlways || state.RemembererSettings == RemembererSettings.StorePerFile;

            return (fileOptimize, dirOptimize);
        }

        private void State_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var optimize = GetRemembererSettings();

            fileRememberer.ShouldAlwaysOptimize = optimize.fileOptimize;
            dirRememberer.ShouldAlwaysOptimize = optimize.dirOptimize;
        }

        private void FilesProcessingState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void ProcessingStateData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StaticState.WindowStateManager.Save();
        }

        public ICommand GoCommand { get; private set; }
        private async Task GoCommandImp()
        {
            var state = StaticState.UserSettingsManager.State;

            var config = state.ToDeveImageOptimizerConfiguration();

            var fileProcessor = new DeveImageOptimizerProcessor(config, FilesProcessingState, fileRememberer, dirRememberer);

            if (!state.ExecuteImageOptimizationParallel)
            {
                await fileProcessor.ProcessDirectory(WindowState.ProcessingDirectory);
            }
            else
            {
                await fileProcessor.ProcessDirectoryParallel(WindowState.ProcessingDirectory, state.MaxDegreeOfParallelism);
            }
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