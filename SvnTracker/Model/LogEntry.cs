using System.Collections.Generic;

namespace SvnTracker.Model
{
    public class LogEntry : IChange
    {
        private readonly string m_CurrentUser;

        public LogEntry(long revision, string currentUser)
        {
            m_CurrentUser = currentUser;
            Revision = revision;
            Files = new List<SvnPath>();
        }

        public string BaseURL { get; set; }
        public long Revision
        {
            get; private set;
        }

        public string User { get; set; }
        public string Message { get; set; }
        public IList<SvnPath> Files { get; set; }

        /// <summary>
        /// If you've checked out trunk and someone copies it to tags that's not relevant.
        /// Or if you are the user that made the change.
        /// </summary>
        public bool IsRelevant
        {
            get
            {
                if (User == m_CurrentUser)
                    return false;

                foreach (var file in Files)
                {
                    if (file.Path.Contains(file.Root))
                        return true;
                }
                return false;
            }
        }
    }
}