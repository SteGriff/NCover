using NUnit.Framework;

namespace SharpCover.Parsing.CSharp
{
	[TestFixture]
	public class ConditionalMatcherTests
	{
		public ConditionalMatcherTests()
		{
		}

		[Test]
		public void TestFailure()
		{
			string input = "for(int i=0;i<20;;i++){ fs }";
			Matcher.Comments = new Comments(input);
			ConditionalMatcher matcher = new ConditionalMatcher(null);
			string output = matcher.Replace(input);

			Assert.AreEqual(input, output);
		}
	}
}
