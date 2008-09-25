namespace SharpCover.Parsing.ProjectFiles
{
	/// <summary>
	/// Finds project files and adds a reference to SharpCover into them.
	/// </summary>
	public class ProjectFileParser : IParse
	{
		#region IParse Members

		public bool Accept(string filename)
		{
			return filename.EndsWith(".csproj") || filename.EndsWith(".vbproj");
		}

		public string Parse(string filename)
		{
			return null;
		}

		public CoveragePoint[] CoveragePoints
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}
