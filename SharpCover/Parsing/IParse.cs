namespace SharpCover.Parsing
{
	public interface IParse
	{
		/// <summary>
		/// Returns true if this parser can parse this type of file.
		/// </summary>
		bool Accept(string filename);

		string Parse(string filename);
		CoveragePoint[] CoveragePoints {get;}
	}
}