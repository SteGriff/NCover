using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
	public class MethodMatcher : Matcher
	{
		public MethodMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
		private string post;
		private Regex noimplementation = new Regex(@"\S+", RegexOptions.Compiled);
		private readonly Regex regex = new Regex(@"(?<Declaration>(public|private|protected|internal)\s+?[^{]+?\([^{]*?\)\s*\{)(?<Implementation>.*?)(})", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

		public override Regex Regex
		{
			get{return this.regex;}
		}


		protected override int GetAbsoluteInsertPoint(Match match)
		{
			if(!this.noimplementation.IsMatch(match.Groups["Implementation"].Value))
				return -1;

			this.pre = match.Groups["Declaration"].Value;
			this.post = match.Groups["Implementation"].Value;

			return match.Index + this.pre.Length;
		}

		protected override string PerformMatch(Match match)
		{				
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + "; " + this.post + "}";
		}
	}
}