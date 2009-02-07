using System.Collections.Generic;
using NUnit.Framework;

namespace SharpCover.Parsing.VB
{
	[TestFixture]
	public class FunctionMatcherTests
	{
		[SetUp]
		public void SetUp()
		{
			i = 0;
			at = new List<int>();
		}

		int i;
		string MockAddPoint(int i)
		{
			Assert.IsTrue(this.i == 0);
			this.i = i;
			return "SharpCover.Parsing.Vb.FunctionMatcherTests.At(" + i + ")";
		}

        static List<int> at;

		public static bool At(int i)
		{
			at.Add(i);
			return true;
		}

        [Test, Ignore("Todo")]
		public void TestFunction()
		{
			const string input = @"
    Public Class A
        Public Shared Function B()
            If System.Environment.TickCount > 0 Then '4
            End If

        End Function
    End Class";

			i = 0;
			TestUtilities.AssertCompilesInVb(input);

			Matcher.Comments = new Comments(input);
			FunctionMatcher matcher = new FunctionMatcher(new AddCoveragePointDelegate(MockAddPoint));
			string output = matcher.Replace(input);
			//Assert.IsTrue(i > 0);

			TestUtilities.AssertCompilesInVb(output);

			//todo check was hit!
			Assert.IsTrue(at.Count == 1);
			Assert.AreEqual(i, at[0]);
		}



        [Test, Ignore("Todo")]
		public void TestFunction2()
		{
			const string input = @"
    Public Class A
        Public Shared Function B() As Integer
            If System.Environment.TickCount > 0 Then '4
            End If

        End Function
    End Class";

			i = 0;
			TestUtilities.AssertCompilesInVb(input);

			Matcher.Comments = new Comments(input);
			FunctionMatcher matcher = new FunctionMatcher(new AddCoveragePointDelegate(MockAddPoint));
			string output = matcher.Replace(input);
			

			TestUtilities.AssertCompilesInVb(output);

			//todo check was hit!
			Assert.IsTrue(at.Count == 1);
			Assert.AreEqual(i, at[0]);
		}
	}
}
