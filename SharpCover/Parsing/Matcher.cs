using System.Text.RegularExpressions;

namespace SharpCover.Parsing
{
    /// <summary>
    /// 
    /// </summary>
	public delegate string AddCoveragePointDelegate(int LineNumber);

    /// <summary>
    /// 
    /// </summary>
	public abstract class Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Matcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public Matcher(AddCoveragePointDelegate AddPointCallback)
		{
			this.addpoint = AddPointCallback;
		}

        /// <summary>
        /// 
        /// </summary>
		protected AddCoveragePointDelegate addpoint;
        /// <summary>
        /// 
        /// </summary>
		protected int absoluteInsertPoint = 0;
		private static IComments comments;

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>The regex.</value>
		public abstract Regex Regex
		{
			get;
		}

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
		public static IComments Comments
		{
			get{return comments;}
			set{comments = value;}
		}

		private string Match(Match match)
		{
			if (!match.Success)
				return match.Value;

			this.absoluteInsertPoint = GetAbsoluteInsertPoint(match);
			
			if(this.absoluteInsertPoint == -1 || comments.IsInComment(absoluteInsertPoint))
				return match.Value;
			else
				return PerformMatch(match);
		}

        /// <summary>
        /// Replaces the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
		public string Replace(string input)
		{
			return this.Regex.Replace(input, new MatchEvaluator(this.Match));
		}

        /// <summary>
        /// Performs the match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected abstract string PerformMatch(Match match);
        /// <summary>
        /// Gets the absolute insert point.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected abstract int GetAbsoluteInsertPoint(Match match);
		
	}
}