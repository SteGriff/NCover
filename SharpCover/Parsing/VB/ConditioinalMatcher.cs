using System;
using System.Text.RegularExpressions;

namespace SharpCover.Parsing.VB
{
	public class ConditionalMatcher : Matcher
	{
		public ConditionalMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(@"(\s+(If|ElseIf)\s)(.*?)(Then)", RegexOptions.Compiled & RegexOptions.Multiline);
		private string pre;

		public override Regex Regex
		{
			get{return this.regex;}
		}

		protected override int GetAbsoluteInsertPoint(Match match)
		{
			this.pre = match.Groups[1].Value;

			return match.Index + this.pre.Length;
		}

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