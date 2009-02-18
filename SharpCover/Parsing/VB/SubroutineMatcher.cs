using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
    /// <summary>
    /// 
    /// </summary>
	public class SubroutineMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SubroutineMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public SubroutineMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
		private static readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+?Shared?\s+Sub\s+[^\(]+[^\)]+\)\s*\r\n)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>The regex.</value>
		public override Regex Regex
		{
			get{return regex;}
		}


        /// <summary>
        /// Gets the absolute insert point.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.pre = match.Groups["Declaration"].Value;

			//too complicated to instrument
			if (pre.TrimEnd().EndsWith("_"))
				return -1;

			return match.Index + this.pre.Length;
		}

        /// <summary>
        /// Performs the match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected override string PerformMatch(Match match)
		{				
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + " : ";
		}
	}
}