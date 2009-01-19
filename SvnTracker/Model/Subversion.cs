using System.Diagnostics;
using System.IO;

namespace SvnTracker.Model
{
    public class Subversion : ISubversion
    {
        public string GetInfo(string workingDir)
        {
            return GetXml(workingDir, "info --xml");
        }

        public string GetInfo(string workingDir, string url)
        {
            return GetXml(workingDir, "info " + url + " --xml ");            
        }

        public string GetLog(string workingDir, string url, long revision)
        {
            return GetXml(workingDir, "log " + url + " -r " + revision + " -v --xml ");
        }

        private static string GetXml(string dir, string args)
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

            return proc.StandardOutput.ReadToEnd();
        }
    }
}