using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SharpCover.Logging;

namespace SharpCover.Parsing.CSharp
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

		private readonly Regex regex = new Regex(@"(\s*(if|while|for)\s*\()(.*?\))", RegexOptions.Compiled);
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
				case "if":
				case "while":
					return MatchIfWhile(semicolon);
				case "for":
					return MatchFor(semicolon, match);
				default:
					throw new ApplicationException("unknown statement type " + statementType);
			}				
		}

		private string MatchFor(string conditionalsToSemicolon, Match match)
		{
			string inner = conditionalsToSemicolon.Substring(0, conditionalsToSemicolon.Length - 1);
			string[] bits = inner.Split(';');

			if (bits.Length != 3)
			{
				Trace.WriteLineIf(Logger.OutputType.TraceError, "Odd for found: for(" + inner +")");
				return match.Value;
			}

			string condition = " && " + bits[1];
			absoluteInsertPoint += bits[0].Length;

			string code = base.addpoint(absoluteInsertPoint);

			return this.pre + bits[0] + ';' + code + condition + ';'	+ bits[2] + ')';
		}
		
		private string MatchIfWhile(string conditionalsToSemicolon)
		{
			string condition = " && " + conditionalsToSemicolon.Substring(0, conditionalsToSemicolon.Length - 1);
	
			string code = base.addpoint(absoluteInsertPoint);
			
			return this.pre + code + condition + ')';
		}
	}
}