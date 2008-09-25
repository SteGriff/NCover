using System;
using NUnit.Framework;

namespace SharpCover.Utilities
{
	[TestFixture]
	public class GradientPointTests
	{
		public GradientPointTests()
		{
		}

		[Test]
		public void TestConstructor()
		{
			GradientPoint point = new GradientPoint(100, 1,2,3);
			Assert.AreEqual(100, point.Percent);
			Assert.AreEqual(1, point.Red);
			Assert.AreEqual(2, point.Green);
			Assert.AreEqual(3, point.Blue);
		}

		[Test]
		public void TestProperties()
		{
			GradientPoint point = new GradientPoint(0,0,0,0);
			point.Percent = 10;
			point.Red = 12;
			point.Green = 13;
			point.Blue = 14;

			Assert.AreEqual(10, point.Percent);
			Assert.AreEqual(12, point.Red);
			Assert.AreEqual(13, point.Green);
			Assert.AreEqual(14, point.Blue);
		}
	}
}
