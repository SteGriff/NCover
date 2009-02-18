using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
    /// <summary>
    /// 
    /// </summary>
	public class StatementMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="StatementMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public StatementMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"([{;}]\s*)((foreach|throw|return)\s)", RegexOptions.Compiled);
		private string pre;
		private string post;

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
			this.pre = match.Groups[1].Value;
			this.post = match.Groups[2].Value;

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
			
			return this.pre + code + "; " + this.post;
		}
	}
}