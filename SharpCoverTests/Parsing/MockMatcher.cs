using System.Text.RegularExpressions;

namespace SharpCover.Parsing
{
	public class MockMatcher : Matcher
	{
		public MockMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(".*", RegexOptions.Compiled | RegexOptions.Singleline);

		private bool insertpointcalled = false;
		private bool matchcalled = false;

		private int insertpointreturnvalue = 0;

		public bool InsertPointCalled
		{
			get{return this.insertpointcalled;}
			set{this.insertpointcalled = value;}
		}

		public bool MatchCalled
		{
			get{return this.matchcalled;}
			set{this.matchcalled = value;}
		}

		public int InsertPointReturnValue
		{
			get{return this.insertpointreturnvalue;}
			set{this.insertpointreturnvalue = value;}
		}

		public override Regex Regex
		{
			get{return this.regex;}
		}

		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.insertpointcalled = true;
			return this.insertpointreturnvalue;
		}

		protected override string PerformMatch(Match match)
		{
			this.matchcalled = true;
			return "foo";
		}
	}
}