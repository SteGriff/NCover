using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
	public class StatementMatcher : Matcher
	{
		public StatementMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"([{;}]\s*)((foreach|throw|return)\s)", RegexOptions.Compiled);
		private string pre;
		private string post;

		public override Regex Regex
		{
			get{return this.regex;}
		}

		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.pre = match.Groups[1].Value;
			this.post = match.Groups[2].Value;

			return match.Index + this.pre.Length;
		}

		protected override string PerformMatch(Match match)
		{				
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + "; " + this.post;
		}
	}
}