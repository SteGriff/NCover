using System.Text.RegularExpressions;

namespace SharpCover.Parsing
{
    /// <summary>
    /// 
    /// </summary>
	public class UnflattenMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UnflattenMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public UnflattenMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(".*", RegexOptions.Compiled | RegexOptions.Singleline);
		private const char LEFTBRACE = System.Char.MaxValue;
		private const char RIGHTBRACE = System.Char.MinValue;


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
			return -2;
		}

        /// <summary>
        /// Performs the match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected override string PerformMatch(Match match)
		{
			string result = match.Value;
			result = result.Replace(LEFTBRACE, '(');
			result = result.Replace(RIGHTBRACE, ')');
			return result;
		}
	}
}