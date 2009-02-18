using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
    /// <summary>
    /// 
    /// </summary>
	public class MethodMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public MethodMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private string pre;
		private string post;
		private Regex noimplementation = new Regex(@"\S+", RegexOptions.Compiled);
		private readonly Regex regex = new Regex(@"(?<Declaration>(public|private|protected|internal)\s+?[^{]+?\([^{]*?\)\s*\{)(?<Implementation>.*?)(})", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>The regex.</value>
		public override Regex Regex
		{
			get{return this.regex;}
		}

        /// <summary>
        /// Gets the absolute insert point.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected override int GetAbsoluteInsertPoint(Match match)
		{
			if(!this.noimplementation.IsMatch(match.Groups["Implementation"].Value))
				return -1;

			this.pre = match.Groups["Declaration"].Value;
			this.post = match.Groups["Implementation"].Value;

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
			
			return this.pre + code + "; " + this.post + "}";
		}
	}
}