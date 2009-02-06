using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpCover.Parsing
{
	public class FlattenMatcher : Matcher
	{
		public FlattenMatcher(AddCoveragePointDelegate AddPointCallback) : base(AddPointCallback)
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
			string original = match.Value;
			if (original.IndexOf(LEFTBRACE) >= 0 || original.IndexOf(RIGHTBRACE) >= 0)
			{
				throw new ApplicationException("Can't use min or max char values in source file.");
			}

			int levelIn = 0;
			StringBuilder result = new StringBuilder();
			for( int i = 0; i < original.Length; i++)
			{
				switch (original[i])
				{
					case '(':
						levelIn++;
						if (levelIn == 1)
							result.Append('(');
						else
							result.Append(LEFTBRACE);
						break;
					case ')':
						levelIn--;
						if (levelIn == 0)
							result.Append(')');
						else
							result.Append(RIGHTBRACE);
						break;
					default:
						result.Append(original[i]);
						break;
				}
			}
			return result.ToString();	
		}
	}
}