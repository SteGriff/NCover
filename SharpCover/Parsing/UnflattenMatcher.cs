using System;
using System.Text.RegularExpressions;
using System.Text;

namespace SharpCover.Parsing
{
	public class UnflattenMatcher : Matcher
	{
		public UnflattenMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
		{
		}

		private readonly Regex regex = new Regex(".*", RegexOptions.Compiled | RegexOptions.Singleline);
		private const char LEFTBRACE = System.Char.MaxValue;
		private const char RIGHTBRACE = System.Char.MinValue;


		public override Regex Regex
		{
			get{return this.regex;}
		}

		protected override int GetAbsoluteInsertPoint(Match match)
		{
			return -2;
		}

		protected override string PerformMatch(Match match)
		{
			string result = match.Value;
			result = result.Replace(LEFTBRACE, '(');
			result = result.Replace(RIGHTBRACE, ')');
			return result;
		}
	}
}