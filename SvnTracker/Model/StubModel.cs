using System.Collections.ObjectModel;
using System.IO;

namespace SvnTracker.Model
{
    /*public class StubModel : IModel
    {
        public event CheckinHandler CheckinOccured;
        public event ConflictHandler ConflictDetected;

        readonly ObservableCollection<IChange> m_Changes = new ObservableCollection<IChange>();

        public ObservableCollection<IChange> OutstandingChanges
        {
            get { return m_Changes; }
        }

        public void FireChange() {
            CheckinOccured(null);
        }
        public void FireConflict()
        {
            ConflictDetected(null,null,WatcherChangeTypes.All);
        }
    }*/
}