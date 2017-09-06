using DeveImageOptimizer;
using DeveImageOptimizer.FileProcessing;
using DeveImageOptimizer.Helpers;
using DeveImageOptimizer.State;
using DeveImageOptimizerWPF.State;
using DeveImageOptimizerWPF.State.MainWindowState;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        public FilesProcessingState FilesProcessingState { get; set; }

        public MainViewModel()
        {
            WindowState = StaticState.WindowStateManager.State;
            FilesProcessingState = StaticState.FilesProcessingStateManager.State;

            WindowState.PropertyChanged += ProcessingStateData_PropertyChanged;
            FilesProcessingState.PropertyChanged += FilesProcessingState_PropertyChanged;

            GoCommand = new RelayCommand(async () => await GoCommandImp(), () => true);
        }

        private void FilesProcessingState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StaticState.FilesProcessingStateManager.Save();
        }

        private void ProcessingStateData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StaticState.WindowStateManager.Save();
        }

        public ICommand GoCommand { get; private set; }
        private async Task GoCommandImp()
        {
            var fileOptimizer = new FileOptimizerProcessor(StaticState.UserSettingsManager.State.FileOptimizerPath, Path.Combine(FolderHelperMethods.EntryAssemblyDirectory.Value, Constants.TempDirectoryName));
            var fileProcessor = new FileProcessor(fileOptimizer, FilesProcessingState, new FileProcessedStateRememberer(false));
            await fileProcessor.ProcessDirectory(WindowState.ProcessingDirectory);
        }
    }
}