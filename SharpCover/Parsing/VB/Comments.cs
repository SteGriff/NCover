using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SharpCover.Parsing.VB
{
    /// <summary>
    /// 
    /// </summary>
	public class Comments : IComments
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Comments"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
		public Comments(string source)
		{
			IdentifyCommentsAndStringLiterals(source);
		}

		static Regex selectSingleLineComments = new Regex("[^\"]'.*", RegexOptions.Multiline | RegexOptions.Compiled);

		private static Regex selectStringLiterals = new Regex("[^'](\"\"|\"" + @".*?[^\\]" + "\")", RegexOptions.Singleline | RegexOptions.Compiled);

		private List<Insert> comments;

        /// <summary>
        /// Gets or sets the comment positions.
        /// </summary>
        /// <value>The comment positions.</value>
        public List<Insert> CommentPositions
		{
			get{return this.comments;}
			set{this.comments = value;}
		}

		private void IdentifyCommentsAndStringLiterals(string original)
		{
            this.comments = new List<Insert>();

			MatchCollection matches = selectStringLiterals.Matches(original);

			foreach (Match match in matches)
			{
				this.comments.Add(new Insert(match.Index + 1, match.Value.Substring(1)));
			}

			matches = selectSingleLineComments.Matches(original);

			foreach (Match match in matches)
			{
				this.comments.Add(new Insert(match.Index + 1, match.Value.Substring(1)));
			}

			//todo: can one do multiline comments in vb?
		}

        /// <summary>
        /// Determines whether [is in comment] [the specified position].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        /// 	<c>true</c> if [is in comment] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
		public bool IsInComment(int position)
		{
			if (comments != null)
				foreach (Insert insert in comments)
				{
					if (insert.InsertPoint <= position && position < (insert.InsertPoint + insert.Length))
						return true;

					//if list is sorted by index we can break once InsertPoint > position
				}

			return false;
		}
	}
}