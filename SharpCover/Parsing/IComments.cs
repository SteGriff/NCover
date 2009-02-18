
namespace SharpCover.Parsing
{
	/// <summary>
	/// Comment provider...
	/// </summary>
	public interface IComments
	{
        /// <summary>
        /// Determines whether [is in comment] [the specified position].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        /// 	<c>true</c> if [is in comment] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
		bool IsInComment(int position);
	}
}
