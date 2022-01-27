using System.Collections.Generic;

namespace DeveImageOptimizerWPF.LogViewerData
{
    public class CollapsibleLogEntry : LogEntry
    {
        public List<LogEntry> Contents { get; set; }
    }
}
