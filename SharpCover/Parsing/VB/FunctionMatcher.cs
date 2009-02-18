using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
    /// <summary>
    /// 
    /// </summary>
	public class FunctionMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public FunctionMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
//		private string post;
//		private Regex noimplementation = new Regex(@"\S+", RegexOptions.Compiled);
		private static readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+?Shared?\s+Function\s+.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Multiline );
		//private readonly Regex regex = new Regex(@"(?<Declaration>(Public|Private)\s+(Sub|Function)\s+?[^(]+?\([^{]*?\)\s*\{)(?<Implementation>.*?)(})", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

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

//			this.post = match.Groups["Implementation"].Value;

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