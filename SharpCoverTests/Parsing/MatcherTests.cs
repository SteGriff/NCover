using System;
using NUnit.Framework;
using SharpCover.Parsing.CSharp;

namespace SharpCover.Parsing
{
	[TestFixture]
	public class MatcherTests
	{
		public MatcherTests()
		{
		}

		[Test]
		public void TestProperties()
		{
			Comments comments = new Comments("");
			Matcher.Comments = comments;
			Assert.AreEqual(comments, Matcher.Comments);
		}

	}
}
