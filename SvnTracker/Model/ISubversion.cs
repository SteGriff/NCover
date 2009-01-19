namespace SvnTracker.Model
{
    public interface ISubversion
    {
        string GetInfo(string workingDir);

        string GetInfo(string workingDir, string url);

        string GetLog(string workingDir, string url, long revision);
    }
}
