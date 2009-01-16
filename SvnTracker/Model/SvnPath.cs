using System.Text;

namespace SvnTracker.Model
{
    public class SvnPath
    {
        public SvnPath(LogEntry logEntry)
        {
            LogEntry = logEntry;
        }

        public LogEntry LogEntry { get; private set; }
        public Action Action { get; set; }
        public string Path { get; set; }

        public string Root
        {
            get
            {
                return Path.Substring(0, Path.Length - RelativePath.Length);
            }
        }

        public string RelativePath
        {
            get
            {
                //Path         HERE/IS/COMMON/klnfdslkfnklsnd  
                //baseURL      skjsjsaldjlajasHERE/IS/COMMON
                string last = LogEntry.BaseURL.Substring(LogEntry.BaseURL.IndexOf(':') + 1 + "//".Length);
                string[] rootSegments = last.Split('/');
                string[] relativeSegments = Path.Substring(1).Split('/');

                int start = 0;
                for (int i = 0; i < rootSegments.Length; i++)
                {
                    if (rootSegments[i] == relativeSegments[0])
                    {
                        start = i;
                        break;
                    }                   
                }

                int j = 0;
                for (int i = start; i < rootSegments.Length && rootSegments[i] == relativeSegments[j]; i++, j++)
                {                       
                }

                var buffer = new StringBuilder();
                for (int k = j; k < relativeSegments.Length; k++)
                {
                    if (buffer.Length != 0)
                    {
                        buffer.Append('/');
                    }
                    buffer.Append(relativeSegments[k]);                    
                }
                return buffer.ToString();
            }
        }
    }
}