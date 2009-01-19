using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace SvnTracker.Model
{
    public class DirModel : INotifyPropertyChanged, IModel, IDisposable
    {
        public event CheckinHandler CheckinOccured;
        public event ConflictHandler ConflictDetected;

        private LocalModificationsMonitor m_Monitor;
        private Thread m_thread;

        private string m_MonitoredDir;
        public string MonitoredDir
        {
            get { return m_MonitoredDir; }
            set { 

                m_MonitoredDir = value;
                OutstandingChanges.Clear();

                if (m_thread == null && m_MonitoredDir != null)
                {
                    m_thread = new Thread(Run) {Name = "SvnTracker", IsBackground = true};
                    m_thread.Start();
                    m_dispatcher = Dispatcher.CurrentDispatcher;
                }
                Changed("MonitoredDir");
            }
        }

        public virtual Visibility ClosableVisibility { get { return Visibility.Visible; } }

        public virtual string URLBase
        {
            get
            {
                if (URL == null) return null;
                var segments = URL.Split('/');
                return segments[segments.Length - 2] + '/' + segments[segments.Length - 1];
            }
        }

        private string m_URL;
        public string URL
        {
            get { return m_URL; }
            set { 
                m_URL = value;
                Changed("URLBase");
                Changed("URL");
            }
        }

        public void Changed(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public bool HasChanges { get { return OutstandingChanges.Any(); } }

        private long m_CurrentRevision;
        public long CurrentRevision
        {
            get { return m_CurrentRevision; }
            private set
            {
                m_CurrentRevision = value;
                Changed("CurrentRevision"); 
            }
        }
        
        readonly ObservableCollection<IChange> m_Changes = new ObservableCollection<IChange>();

        public ObservableCollection<IChange> OutstandingChanges
        {
            get { return m_Changes; }
        }

        public void Run()
        {
            while (!Cancelled)
            {

                Thread.Sleep(1000);
                Update();
                Thread.Sleep(ModelFactory.Instance.PollIntervalInSeconds * 1000);
            }
        }

        public bool Cancelled { get; set; }

        private void Update()
        {
            try
            {
                MonitorDir(MonitoredDir);
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + e.StackTrace);
            }
        }

        readonly Dictionary<string, LogEntry> m_UrlRevisionCache = new Dictionary<string, LogEntry>();
        private Dispatcher m_dispatcher;

        private string m_CurrentUser;
        public string CurrentUser
        {
            get { return m_CurrentUser ?? Environment.UserName; }
            set { m_CurrentUser = value; }
        }

        private void MonitorDir(string dir)
        {
            var xdoc = GetXml(dir, "info --xml");

            const string xpath = "/info/entry[1]/url/text()";
            const string xpath2 = "/info/entry[1]/@revision";

            URL = xdoc.SelectSingleNode(xpath).Value;
            CurrentRevision = Int64.Parse(xdoc.SelectSingleNode(xpath2).Value);
            MaxKnownRevision = Math.Max(CurrentRevision, MaxKnownRevision);
            
            xdoc = GetXml(dir, "info " + URL + " --xml ");
            int repositoryRevision = Int32.Parse(xdoc.SelectSingleNode(xpath2).Value);

            if (repositoryRevision <= MaxKnownRevision)
            {
                return;
            }

            var x = new NewList();
            for (long revision = MaxKnownRevision + 1; revision <= repositoryRevision; revision++)
            {
                LogEntry logEntry;
                var key = URL + "@" + revision;
                if (!m_UrlRevisionCache.TryGetValue(key, out logEntry))
                {
                    logEntry = GetLogEntry(dir, URL, revision);
                    if (logEntry != null)
                    {
                        m_UrlRevisionCache.Add(key, logEntry);
                    }
                }
                if (logEntry != null)
                {
                    x.Entries.Add(logEntry);
                }
            }
            MaxKnownRevision = repositoryRevision;
            m_dispatcher.Invoke(new UpdateUIDelegate(UpdateUI),new object[] { x });            
        }

        private long MaxKnownRevision { get; set; }

        public class NewList
        {
            public List<LogEntry> Entries = new List<LogEntry>();
        }

        public delegate void UpdateUIDelegate(NewList args);
        private void UpdateUI(NewList args)
        {
            OutstandingChanges.Clear();
            args.Entries.ForEach(OutstandingChanges.Add);

            if (m_Monitor != null)
            {
                m_Monitor.Dispose();
            }
            m_Monitor = new LocalModificationsMonitor(this, MonitoredDir, OutstandingChanges.SelectMany(change => change.Files));

            if (CheckinOccured != null)
                CheckinOccured(OutstandingChanges);

            Changed("HasChanges");
        }

        private LogEntry GetLogEntry(string dir, string url, long revision)
        {
            XmlDocument xdoc = GetXml(dir, "log " + url + " -r " + revision + " -v --xml ");

            var messageNode = xdoc.SelectSingleNode("/log/logentry/msg/text()");
            var authorNode = xdoc.SelectSingleNode("/log/logentry/author/text()");
            if (authorNode == null)
            {
                return null;    
            }

            var logEntry = new LogEntry(revision, CurrentUser)
                               {
                                   BaseURL = url,
                                   Message = messageNode == null? "Naughty " + authorNode.Value + " didn't leave a log message": messageNode.Value,
                                   User = authorNode.Value
                               };

            var xmlNodeList = xdoc.SelectNodes("/log/logentry/paths/path");
            if (xmlNodeList == null) return null;
            foreach (XmlElement xnav in xmlNodeList)
            {
                //Path: need to remove /example/trunk from path...
                var item = new SvnPath(logEntry)
                               {
                                   Action = Parse(xnav.SelectSingleNode("@action").Value),
                                   Path = xnav.SelectSingleNode("text()").Value
                               };
                logEntry.Files.Add(item);
            }
            return logEntry;
        }

        private static Action Parse(string value)
        {
            switch (value)
            {
                case "A":
                    return Action.Added;
                case "M":
                    return Action.Modified;
                case "D":
                    return Action.Deleted;
                default:
                    throw new Exception("Unknown action " + value);
            }
        }

        private static XmlDocument GetXml(string dir, string args)
        {
            var settings = new ProcessStartInfo
                               {
                                   FileName = Path.Combine(ModelFactory.Instance.SvnDir, "svn.exe"),
                                   Arguments = args,
                                   UseShellExecute = false,
                                   WorkingDirectory = dir,
                                   RedirectStandardOutput = true,
                                   CreateNoWindow = true
                               };
            var proc = Process.Start(settings);
            if (proc == null) return null;
            proc.WaitForExit();

            var xdoc = new XmlDocument();
            xdoc.LoadXml(proc.StandardOutput.ReadToEnd());
            return xdoc;
        }

        public void RaiseConflictDetected(SvnPath path, string file, WatcherChangeTypes changeType)
        {
            if (ConflictDetected != null)
            {
                ConflictDetected(path, file, changeType);
            }            
        }

        public void Dispose()
        {
            if (m_Monitor == null)
                return;

            m_Monitor.Dispose();
            m_Monitor = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}