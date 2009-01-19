using System.Collections.Generic;

namespace SvnTracker.Model
{
    public class Stubversion : Dictionary<string,string>, ISubversion
    {
        public string GetInfo(string workingDir)
        {
            return GetKey(workingDir);
        }

        public string GetInfo(string workingDir, string url)
        {
            return GetKey(url);
        }

        public string GetLog(string workingDir, string url, long revision)
        {
            return GetKey(url + "@" + revision);
        }

        private string GetKey(string workingDir)
        {
            if (!ContainsKey(workingDir)) throw new KeyNotFoundException("Key not found: " + workingDir);
            return this[workingDir];
        }
    }
}