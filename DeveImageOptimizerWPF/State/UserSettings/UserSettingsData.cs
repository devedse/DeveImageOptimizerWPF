using PropertyChanged;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeveImageOptimizerWPF.State.UserSettings
{
    [Serializable]
    [ImplementPropertyChanged]
    public class UserSettingsData : IChangable
    {
        public String FileOptimizerPath { get; set; } = string.Empty;
        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}
