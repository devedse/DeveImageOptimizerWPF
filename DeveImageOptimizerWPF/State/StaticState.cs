using DeveImageOptimizer.Helpers;
using DeveImageOptimizerWPF.State.MainWindowState;
using DeveImageOptimizerWPF.State.UserSettings;
using System.IO;

namespace DeveImageOptimizerWPF.State
{
    public static class StaticState
    {
        public static StateManager<UserSettingsData> UserSettingsManager { get; } = new StateManager<UserSettingsData>(Path.Combine(FolderHelperMethods.ConfigFolder, "UserSettings.xml"));
        public static StateManager<WindowState> WindowStateManager { get; } = new StateManager<WindowState>(Path.Combine(FolderHelperMethods.ConfigFolder, "WindowState.xml"));

        //Commented out because we don't need to save all processed files (this is only UI stuff)
        //public static StateManager<FilesProcessingState> FilesProcessingStateManager { get; } = new StateManager<FilesProcessingState>(Path.Combine(FolderHelperMethods.ConfigFolder, "FilesProcessingState.xml"));
    }
}
