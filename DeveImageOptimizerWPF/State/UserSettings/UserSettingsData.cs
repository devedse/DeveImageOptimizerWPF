using DeveImageOptimizer.FileProcessing;
using DeveImageOptimizer.ImageOptimization;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeveImageOptimizerWPF.State.UserSettings
{
    [Serializable]
    public class UserSettingsData : IChangable, INotifyPropertyChanged
    {
        public string FileOptimizerPath { get; set; }

        public string TempDirectory { get; set; }

        public bool HideOptimizerWindow { get; set; }

        public RemembererSettings RemembererSettings { get; set; }

        public bool SaveFailedFiles { get; set; }
        public bool KeepFileAttributes { get; set; }

        public bool ExecuteImageOptimizationParallel { get; set; }
        public int MaxDegreeOfParallelism { get; set; }

        public bool DirectlyCallOptimizers { get; set; }

        public ImageOptimizationLevel ImageOptimizationLevel { get; set; }

        public bool OptimizeJpg { get; set; }
        public bool OptimizePng { get; set; }
        public bool OptimizeGif { get; set; }
        public bool OptimizeBmp { get; set; }

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
            HideOptimizerWindow = config.HideOptimizerWindow;
            SaveFailedFiles = config.SaveFailedFiles;
            KeepFileAttributes = config.KeepFileAttributes;

            ExecuteImageOptimizationParallel = config.ExecuteImageOptimizationParallel;
            MaxDegreeOfParallelism = config.MaxDegreeOfParallelism;

            DirectlyCallOptimizers = config.CallOptimizationToolsDirectlyInsteadOfThroughFileOptimizer;

            ImageOptimizationLevel = config.ImageOptimizationLevel;

            OptimizeJpg = config.OptimizeJpg;
            OptimizePng = config.OptimizePng;
            OptimizeGif = config.OptimizeGif;
            OptimizeBmp = config.OptimizeBmp;

            LogLevel = config.LogLevel;
        }

        public DeveImageOptimizerConfiguration ToDeveImageOptimizerConfiguration()
        {
            var config = new DeveImageOptimizerConfiguration()
            {
                ExecuteImageOptimizationParallel = ExecuteImageOptimizationParallel,
                FileOptimizerPath = FileOptimizerPath,
                HideOptimizerWindow = HideOptimizerWindow,
                LogLevel = LogLevel,
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                SaveFailedFiles = SaveFailedFiles,
                KeepFileAttributes = KeepFileAttributes,
                TempDirectory = TempDirectory,
                CallOptimizationToolsDirectlyInsteadOfThroughFileOptimizer = DirectlyCallOptimizers,
                ImageOptimizationLevel = ImageOptimizationLevel,
                OptimizeJpg = OptimizeJpg,
                OptimizePng = OptimizePng,
                OptimizeGif = OptimizeGif,
                OptimizeBmp = OptimizeBmp
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
