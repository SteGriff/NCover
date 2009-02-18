using System;
using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
    /// <summary>
    /// 
    /// </summary>
	public class ConditionalMatcher : Matcher
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalMatcher"/> class.
        /// </summary>
        /// <param name="AddPointCallback">The add point callback.</param>
		public ConditionalMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"(\s+(If|ElseIf)\s)(.*?)(Then)", RegexOptions.Compiled & RegexOptions.Multiline);
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
			this.pre = match.Groups[1].Value;

			return match.Index + this.pre.Length;
		}

        /// <summary>
        /// Performs the match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
		protected override string PerformMatch(Match match)
		{	
			string semicolon = match.Groups[3].Value;
			string statementType = match.Groups[2].Value;
			
			switch(statementType)
			{
				case "If":
				case "ElseIf":
					return MatchIf(semicolon);
				default:
					throw new ApplicationException("unknown statement type " + statementType);
			}				
		}

		private string MatchIf(string conditionalsToSemicolon)
		{
			string condition = conditionalsToSemicolon.Substring(0, conditionalsToSemicolon.Length - 1);
	
			string code = base.addpoint(absoluteInsertPoint);
			 
			return this.pre + condition + " AndAlso " + code + " Then ";
		}
	}
}