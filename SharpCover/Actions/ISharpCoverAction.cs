using System.Collections.Specialized;

namespace SharpCover.Actions
{
    /// <summary>
    /// 
    /// </summary>
	public interface ISharpCoverAction
	{
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
		ReportSettings Settings {get;}
        /// <summary>
        /// Gets or sets the filenames.
        /// </summary>
        /// <value>The filenames.</value>
		StringCollection Filenames {get; set;}
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
		decimal Execute();
	}
}
