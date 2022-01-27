using System;

namespace DeveImageOptimizerWPF.LogViewerData
{
    public class LogEntry : PropertyChangedBase
    {
        public DateTime DateTime { get; set; }

        public int Index { get; set; }

        public string Message { get; set; }
    }
}
