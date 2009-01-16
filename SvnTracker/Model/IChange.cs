using System.Collections.Generic;

namespace SvnTracker.Model
{
    public interface IChange
    {
        long Revision { get; }
        string User { get; }
        string Message { get; }
        IList<SvnPath> Files { get; }        
    }
}