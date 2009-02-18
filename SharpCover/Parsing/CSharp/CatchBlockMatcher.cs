using System.Text.RegularExpressions;

namespace SharpCover.Parsing.CSharp
{
    /// <summary>
    /// 
    /// </summary>
	public class CatchBlockMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CatchBlockMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public CatchBlockMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"\s*catch\s*(\(.*?\)){0,1}\s*{", RegexOptions.Compiled);
		private string pre;

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
			this.pre = match.Value;
			
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
			
			return this.pre + code + "; ";
		}
	}
}