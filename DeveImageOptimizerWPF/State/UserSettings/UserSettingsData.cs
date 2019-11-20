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

        public void ResetToDefaults()
        {
            FileOptimizerPath = @"C:\Program Files\FileOptimizer\FileOptimizer64.exe";
            HideFileOptimizerWindow = true;
            RemembererSettings = RemembererSettings.StorePerFileAndDirectory;
            SaveFailedFiles = false;
            ExecuteImageOptimizationParallel = true;
            MaxDegreeOfParallelism = 4;
            LogLevel = 2;
        }

        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}
