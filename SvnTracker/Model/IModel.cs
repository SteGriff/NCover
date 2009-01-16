using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SvnTracker.Model
{
    public delegate void CheckinHandler(IList<IChange> changes);
    public delegate void ConflictHandler(SvnPath filename, string file, WatcherChangeTypes changeType);

    /// <summary>
    /// look at svn dir to determin if up to date.
    /// Warn if incoming modifications.
    /// Error if not up to date file is modified.
    /// </summary>
    interface IModel
    {
        event CheckinHandler CheckinOccured;
        event ConflictHandler ConflictDetected;
        string MonitoredDir { get; set; }
        ObservableCollection<IChange> OutstandingChanges { get; }
    }

    public enum Action { Added, Modified, Deleted }
}
