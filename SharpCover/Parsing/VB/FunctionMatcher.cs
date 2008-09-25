using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
	public class FunctionMatcher : Matcher
	{
		public FunctionMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
//		private string post;
//		private Regex noimplementation = new Regex(@"\S+", RegexOptions.Compiled);
		private static readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+?Shared?\s+Function\s+.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Multiline );
		//private readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+(Sub|Function)\s+?[^(]+?\([^{]*?\)\s*\{)(?<Implementation>.*?)(})", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

		public override Regex Regex
		{
			get{return regex;}
		}


		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.pre = match.Groups["Declaration"].Value;

			//too complicated to instrument
			if (pre.TrimEnd().EndsWith("_"))
				return -1;

//			this.post = match.Groups["Implementation"].Value;

			return match.Index + this.pre.Length;
		}

		protected override string PerformMatch(Match match)
		{				
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + " : ";
		}
	}
}