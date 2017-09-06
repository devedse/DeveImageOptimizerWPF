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
        public String FileOptimizerPath { get; set; } = @"C:\Program Files\FileOptimizer\FileOptimizer64.exe";
        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}
