namespace SharpCover.Parsing
{
    /// <summary>
    /// 
    /// </summary>
	public interface IParse
	{
		/// <summary>
		/// Returns true if this parser can parse this type of file.
		/// </summary>
		bool Accept(string filename);

        /// <summary>
        /// Parses the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
		string Parse(string filename);

        /// <summary>
        /// Gets the coverage points.
        /// </summary>
        /// <value>The coverage points.</value>
		CoveragePoint[] CoveragePoints {get;}
	}
}