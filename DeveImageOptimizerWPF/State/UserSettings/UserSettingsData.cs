using DeveImageOptimizer.FileProcessing;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeveImageOptimizerWPF.State.UserSettings
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class UserSettingsData : IChangable, INotifyPropertyChanged
    {
        public string FileOptimizerPath { get; set; }

        public string TempDirectory { get; set; }

        public bool HideFileOptimizerWindow { get; set; }

        public RemembererSettings RemembererSettings { get; set; }

        public bool SaveFailedFiles { get; set; }

        public bool ExecuteImageOptimizationParallel { get; set; }
        public int MaxDegreeOfParallelism { get; set; }

        public int LogLevel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public UserSettingsData()
        {
            ResetToDefaults();
        }

        public void LoadFromConfiguration(DeveImageOptimizerConfiguration config)
        {
            FileOptimizerPath = config.FileOptimizerPath;
            TempDirectory = config.TempDirectory;
            HideFileOptimizerWindow = config.HideFileOptimizerWindow;
            SaveFailedFiles = config.SaveFailedFiles;

            ExecuteImageOptimizationParallel = config.ExecuteImageOptimizationParallel;
            MaxDegreeOfParallelism = config.MaxDegreeOfParallelism;

            LogLevel = config.LogLevel;
        }

        public DeveImageOptimizerConfiguration ToDeveImageOptimizerConfiguration()
        {
            var config = new DeveImageOptimizerConfiguration()
            {
                ExecuteImageOptimizationParallel = ExecuteImageOptimizationParallel,
                FileOptimizerPath = FileOptimizerPath,
                HideFileOptimizerWindow = HideFileOptimizerWindow,
                LogLevel = LogLevel,
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                SaveFailedFiles = SaveFailedFiles,
                TempDirectory = TempDirectory
            };
            return config;
        }

        public void ResetToDefaults()
        {
            RemembererSettings = RemembererSettings.StorePerFile;
            LoadFromConfiguration(new DeveImageOptimizerConfiguration());
        }

        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}
