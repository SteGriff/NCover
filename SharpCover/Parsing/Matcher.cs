using System.Text.RegularExpressions;

namespace SharpCover.Parsing
{	
	public delegate string AddCoveragePointDelegate(int LineNumber);

	public abstract class Matcher
	{
		public Matcher(AddCoveragePointDelegate AddPointCallback)
		{
			this.addpoint = AddPointCallback;
		}

		protected AddCoveragePointDelegate addpoint;
		protected int absoluteInsertPoint = 0;
		private static IComments comments;
		
		public abstract Regex Regex
		{
			get;
		}
		
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

		public string Replace(string input)
		{
			return this.Regex.Replace(input, new MatchEvaluator(this.Match));
		}

		protected abstract string PerformMatch(Match match);
		protected abstract int GetAbsoluteInsertPoint(Match match);
		
	}
}