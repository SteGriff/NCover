using System.Collections.Generic;
using NUnit.Framework;

namespace SharpCover.Parsing.VB
{
	[TestFixture]
	public class SubroutineMatcherTests
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
			return "SharpCover.Parsing.Vb.SubroutineMatcherTests.At(" + i + ")";
		}

        static List<int> at;

		public static bool At(int i)
		{
			at.Add(i);
			return true;
		}

		[Test,Ignore("Todo")]
		public void TestSubroutine()
		{
			const string input = @"
    Public Class A
        Public Shared Sub B()
            If System.Environment.TickCount > 0 Then '4
            End If

        End Sub
    End Class";

			TestUtilities.AssertCompilesInVb(input);

			Matcher.Comments = new Comments(input);
			var matcher = new SubroutineMatcher(MockAddPoint);
			string output = matcher.Replace(input);
			
			TestUtilities.AssertCompilesInVb(output);

			//todo check was hit!
			Assert.IsTrue(at.Count == 1);
			Assert.AreEqual(i, at[0]);
		}
	}
}
