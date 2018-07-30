using PropertyChanged;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeveImageOptimizerWPF.State.UserSettings
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class UserSettingsData : IChangable
    {
        public String FileOptimizerPath { get; set; }

        public bool HideFileOptimizerWindow { get; set; }

        public bool ForceOptimizeEvenIfAlreadyOptimized { get; set; }

        public UserSettingsData()
        {
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            FileOptimizerPath = @"C:\Program Files\FileOptimizer\FileOptimizer64.exe";
            HideFileOptimizerWindow = true;
            ForceOptimizeEvenIfAlreadyOptimized = false;
        }

        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}
