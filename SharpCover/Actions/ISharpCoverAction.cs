using System.Collections.Specialized;

namespace SharpCover.Actions
{
	public interface ISharpCoverAction
	{
		ReportSettings Settings {get;}
		StringCollection Filenames {get; set;}
		decimal Execute();
	}
}
