using System.Collections.Generic;

namespace SvnTracker.Model
{
    public class Settings {
        //public IList<string> MonitoredDirectories { get; set; }
        public string SvnDir { get; set; }

        public int PollIntervalInSeconds { get; set; }
    }
}