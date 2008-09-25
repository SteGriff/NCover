using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
	public class SubroutineMatcher : Matcher
	{
		public SubroutineMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
		private static readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+?Shared?\s+Sub\s+[^\(]+[^\)]+\)\s*\r\n)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

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

			return match.Index + this.pre.Length;
		}

		protected override string PerformMatch(Match match)
		{				
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + " : ";
		}
	}
}