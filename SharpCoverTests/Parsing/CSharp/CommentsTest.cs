using System;
using System.Collections;
using NUnit.Framework;

namespace SharpCover.Parsing.CSharp
{
	[TestFixture]
	public class CommentsTest
	{
		[Test]
		public void IdentifiesSingleLineComments() 
		{
			string pre = "not comment";
			string comment = "// this is one type of comment if ( ) .";
			string post = "\nnot comment	";
			
			string example = pre + comment + post;

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			Assert.AreEqual(1, commentPositions.Count);

			Insert insert = (Insert)commentPositions[0];
			Assert.AreEqual(pre.Length, insert.InsertPoint);
			Assert.AreEqual(comment.Length, insert.Length);
		}

		[Test]
		public void IdentifiesMultilineComments() 
		{
			string pre = "not comment	";
			string comment = @"/*	a second, - multiline
						comment */";
			string post = @" try to trick it with a false comment end */ 
not comment
";
			string example = pre + comment + post;

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			
			Assert.AreEqual(1, commentPositions.Count);

			Insert insert = (Insert)commentPositions[0];
			Assert.AreEqual(pre.Length, insert.InsertPoint);
			Assert.AreEqual(comment.Length, insert.Length);
		}

		[Test]
		public void identificationOfTwoStringLiterals()
		{
			string preComment = " hello, here is one type of ";
			string comment1 = "\"comment\"";
			string postComment1 = ", and here's ";
			string comment2 = "\"another comment\"";
			string postComment2 = ".";

			string example = preComment + comment1 + postComment1 + comment2 + postComment2; 

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			
			Assert.AreEqual(2, commentPositions.Count);
			Insert insert1 = (Insert)commentPositions[0];
			Assert.AreEqual(preComment.Length, insert1.InsertPoint);
			Assert.AreEqual(comment1.Length, insert1.Length);
	
			Insert insert2 = (Insert)commentPositions[1];
			Assert.AreEqual((preComment + comment1 + postComment1).Length, insert2.InsertPoint);
			Assert.AreEqual(comment2.Length, insert2.Length);
		}

		[Test]
		public void identificationOfStringLiteralsWithLineBreaks()
		{
			string preComment = " hello, here is one type of ";
			string comment = "\"comm\r\nent\"";
			string postComment = ".";

			string example = preComment + comment + postComment; 

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			
			Assert.AreEqual(1, commentPositions.Count);
			Assert.AreEqual(preComment.Length, ((Insert)commentPositions[0]).InsertPoint);
			Assert.AreEqual(comment.Length, ((Insert)commentPositions[0]).Length);
		}

		[Test]
		public void identificationOfStringLiteralsDontGetConfusedWithCharInvertedCommers()
		{
			string preComment = " hello, a '\"' here is one type of ";
			string comment = "\"comm\\\"ent\"";
			string postComment = ".";
			string example = preComment + comment + postComment; 

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			
			Assert.AreEqual(1, commentPositions.Count);
			Assert.AreEqual(preComment.Length, ((Insert)commentPositions[0]).InsertPoint);
			Assert.AreEqual(comment.Length, ((Insert)commentPositions[0]).Length);
		}

		[Test]
		public void identificationOfStringLiteralWithEncodedInvertedCommers()
		{
			string preComment = " hello, here is one type of ";
			string comment = "\"comm\\\"ent\"";
			string postComment = ".";

			string example = preComment + comment + postComment; 

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			
			Assert.AreEqual(1, commentPositions.Count);
			Assert.AreEqual(preComment.Length, ((Insert)commentPositions[0]).InsertPoint);
			Assert.AreEqual(comment.Length, ((Insert)commentPositions[0]).Length);
		}
	
		
		[Test]
		public void StringLiteralGoingWrongBug()
		{
			//writer.WriteString(". (" + report.HitCoveragePoints.ToString() + "/" + report.TotalCoveragePoints + ")");

			//pre("/" post
			string example = "pre(\"/\" post";

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			Assert.AreEqual(1, commentPositions.Count);
			Assert.AreEqual("\"/\"", ((Insert)commentPositions[0]).SelectedArea);
			Assert.AreEqual(3, ((Insert)commentPositions[0]).Length);
			Assert.AreEqual(4, ((Insert)commentPositions[0]).InsertPoint);
		}

		[Test]
		public void NoLengthStringLiteral()
		{
			string example = "pre\"\" middle \"\" post";

			Comments comments = new Comments(example);
			IList commentPositions = comments.CommentPositions;
			Assert.AreEqual(2, commentPositions.Count);

			Assert.AreEqual(2, ((Insert)commentPositions[0]).Length);
			Assert.AreEqual(3, ((Insert)commentPositions[0]).InsertPoint);
			
			Assert.AreEqual(2, ((Insert)commentPositions[1]).Length);
			Assert.AreEqual(13, ((Insert)commentPositions[1]).InsertPoint);
		}
	}
}
