using System.Collections;
using NUnit.Framework;

namespace SharpCover.Parsing.VB
{
	[TestFixture]
	public class ConditionalMatcherTests
	{
		[SetUp]
		public void SetUp()
		{
			i = 0;
			at = new ArrayList();
		}

		int i;
		string MockAddPoint(int i)
		{
			Assert.IsTrue(this.i == 0);
			this.i = i;
			return "SharpCover.Parsing.Vb.ConditionalMatcherTests.At(" + i + ")";
		}

		static ArrayList at;
		public static bool At(int i)
		{
			at.Add(i);
			return true;
		}

        [Test, Ignore("Todo")]
		public void TestIfTrue()
		{
			const string input = @"
    Public Class A
        Public Shared Function B() As Boolean
            If System.Environment.TickCount > 0 Then '4
                Return True 'todo
            End If

            Return False 'todo
        End Function
    End Class";

			TestUtilities.AssertCompilesInVb(input);

			Matcher.Comments = new Comments(input);
			ConditionalMatcher matcher = new ConditionalMatcher(new AddCoveragePointDelegate(MockAddPoint));
			string output = matcher.Replace(input);
			Assert.IsTrue(i > 0);

			Assert.IsTrue((bool) TestUtilities.AssertCompilesInVb(output));

			//todo check was hit!
			Assert.IsTrue(at.Count == 1);
			Assert.AreEqual(i, at[0]);
		}

        [Test, Ignore("Todo")]
		public void TestIfFalse()
		{
			const string input = @"
    Public Class A
        Public Shared Function B() As Boolean
            If System.Environment.TickCount < 0 Then '4
                Return True 'todo
            End If

            Return False 'todo
        End Function
    End Class";

			TestUtilities.AssertCompilesInVb(input);

			Matcher.Comments = new Comments(input);
			ConditionalMatcher matcher = new ConditionalMatcher(new AddCoveragePointDelegate(MockAddPoint));
			string output = matcher.Replace(input);
			Assert.IsTrue(i > 0);

			Assert.IsFalse((bool) TestUtilities.AssertCompilesInVb(output));
			Assert.AreEqual(0, at.Count);
		}

	}
}
