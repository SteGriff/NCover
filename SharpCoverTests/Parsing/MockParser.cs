namespace SharpCover.Parsing
{
	public class MockParser : IParse
	{
		public const string PARSED_TEXT =  "This is the parsed text";
		public MockParser()
		{
		}

		private CoveragePoint[] points = new CoveragePoint[0];

		public CoveragePoint[] CoveragePoints
		{
			get{return this.points;}
			set{this.points = value;}
		}

		public bool Accept(string filename)
		{
			return true;
		}

		public string Parse(string filename)
		{
			return PARSED_TEXT;
		}
	}
}
