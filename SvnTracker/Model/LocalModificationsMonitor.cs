using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SvnTracker.Model
{
    class ComplexPath
    {
        public ComplexPath(string path)
        {
            
            string[] segments = path.Split('\\');
            
            for (int i = 0; i < segments.Length; i++)
            {
                string checkPath = Real == null? segments[0] + @"\" : Path.Combine(Real, segments[i]);
                if (!File.Exists(checkPath) && !Directory.Exists(checkPath) )
                {
                    Imaginary = GetImaginary(i, segments);
                    return;
                }
                Real = checkPath;
            }
        }

        private static string GetImaginary(int i, string[] segments)
        {
            var buffer = new StringBuilder();
            for (int j = i; j < segments.Length; j++)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append('/');
                }
                buffer.Append(segments[j]);
            }
            return buffer.ToString();
        }

        public string Imaginary { get; private set; }
        public string Real { get; private set; }
    }

    public class LocalModificationsMonitor
    {
        private readonly DirModel m_DirModel;
        private readonly List<FileSystemWatcher> m_FileSystemWatchers = new List<FileSystemWatcher>();
        private readonly Dictionary<string, SvnPath> map;

        public LocalModificationsMonitor(DirModel dirModel, string physicalRoot, IEnumerable<SvnPath> list)
        {
            map = new Dictionary<string, SvnPath>();
            m_DirModel = dirModel;
            foreach (var svnPath in list)
            {
//                if (svnPath.Action != Action.Modified && svnPath.Action != Action.Deleted) 
//                    continue;

                var path = Path.Combine(physicalRoot, svnPath.RelativePath);
//                if (!File.Exists(path) || Directory.Exists(path)) 
//                    continue;

                var cp = new ComplexPath(path);
                FileSystemWatcher watcher;
                map[path] = svnPath;
                    
                if (cp.Imaginary != null)
                {
                    watcher = new FileSystemWatcher(cp.Real, cp.Imaginary);                    
                }
                else
                {
                    if (File.Exists(path))
                    {
                        var path1 = new FileInfo(path);
                        watcher = new FileSystemWatcher(path1.DirectoryName, path1.Name);
                    }
                    else
                    {
                        watcher = new FileSystemWatcher(path);
                    }                    
                }
                watcher.Changed += ConflictHandler;
                watcher.Created += ConflictHandler;
                watcher.Deleted += ConflictHandler;
                watcher.IncludeSubdirectories = false;
                watcher.EnableRaisingEvents = true;
                
                m_FileSystemWatchers.Add(watcher);                                    
            }
        }

        private void ConflictHandler(object sender, FileSystemEventArgs e)
        {
            m_DirModel.RaiseConflictDetected(map[e.FullPath], e.FullPath, e.ChangeType);
        }

        public void Dispose()
        {
            foreach (var watcher in m_FileSystemWatchers)
            {
                watcher.Changed -= ConflictHandler;
                watcher.Dispose();
            }
            m_FileSystemWatchers.Clear();
        }
    }
}