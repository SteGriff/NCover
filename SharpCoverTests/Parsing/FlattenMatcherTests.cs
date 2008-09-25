using System;
using NUnit.Framework;
using SharpCover.Parsing.CSharp;

namespace SharpCover.Parsing
{
	[TestFixture]
	public class FlattenMatcherTests
	{
		public FlattenMatcherTests()
		{
		}

		[Test]
		[ExpectedException(typeof(ApplicationException))]
		public void TestFailure()
		{
			string input = "foo bar " + Char.MaxValue.ToString();
			Matcher.Comments = new Comments(input);
			FlattenMatcher matcher = new FlattenMatcher(null);
			matcher.Replace(input);
		}
	}
}
