using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
	public class CatchBlockMatcher : Matcher
	{
		public CatchBlockMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"\s*catch\s*(\(.*?\)){0,1}\s*{", RegexOptions.Compiled);
		private string pre;

		public override Regex Regex
		{
			get{return this.regex;}
		}

		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.pre = match.Value;
			
			return match.Index + this.pre.Length;
		}

		protected override string PerformMatch(Match match)
		{
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + "; ";
		}
	}
}