using System;
using NUnit.Framework;
using SharpCover.Parsing.CSharp;

namespace SharpCover
{
	[TestFixture]
	public class CoveragePointTest
	{
		public CoveragePointTest()
		{
		}

		private CoveragePoint point;
		private CoveragePoint point2;

		[SetUp]
		public void SetUp()
		{
			this.point = new CoveragePoint();
			this.point2 = new CoveragePoint();
		}

		[Test]
		public void TestDefaults()
		{
			Assert.AreEqual(-1, this.point.AbsoluteNumber);
			Assert.AreEqual(-1, this.point.RelativeNumber);
			Assert.IsNull(this.point.Filename);
			Assert.IsNull(this.point.Namespace);
			Assert.IsFalse(this.point.Hit);
			Assert.AreEqual(0, this.point.LineNumber);
		}

		[Test]
		public void TestConstructor()
		{
			this.point = new CoveragePoint("foo.txt", "bar", 1, 1, true);
		
			Assert.AreEqual(2, this.point.AbsoluteNumber);
			Assert.AreEqual(1, this.point.RelativeNumber);
			Assert.AreEqual("foo.txt", this.point.Filename);
			Assert.AreEqual("bar", this.point.Namespace);
			Assert.IsTrue(this.point.Hit);
		}

		[Test]
		public void TestProperties()
		{
			this.point.AbsoluteNumber = 2;
			this.point.RelativeNumber = 3;
			this.point.LineNumber = 4;
			this.point.Filename = "foo.txt";
			this.point.Namespace = "bar";
			this.point.Hit = true;

			Assert.AreEqual(2, this.point.AbsoluteNumber);
			Assert.AreEqual(3, this.point.RelativeNumber);
			Assert.AreEqual("foo.txt", this.point.Filename);
			Assert.AreEqual("bar", this.point.Namespace);
			Assert.IsTrue(this.point.Hit);
			Assert.AreEqual(4, this.point.LineNumber);
		}

		[Test]
		public void TestHashcode()
		{
			this.point.AbsoluteNumber = 5;
			Assert.AreEqual(5, this.point.GetHashCode());
			this.point.AbsoluteNumber = 2;
			Assert.AreEqual(2, this.point.GetHashCode());
		}

		[Test]
		public void TestToString()
		{
			string str = "foo.txt covered by 2 at 20";

			this.point.Filename = "foo.txt";
			this.point.AbsoluteNumber = 2;
			this.point.LineNumber = 20;

			Assert.AreEqual(str, this.point.ToString());
		}

		[Test]
		public void TestGetCoverageCode()
		{
			string str = "SharpCover.Results.Add(@\"foo\", @\"" + Environment.CurrentDirectory + "\\bar\\foo-actual.xml\", 20)";
			
			ReportSettings settings = new ReportSettings();
			settings.ReportName = "foo";
			settings.ReportDir = "bar";

			this.point.AbsoluteNumber = 20;

			Parser parser = new Parser(settings);
			Assert.AreEqual(str, parser.GetCoverageCode(settings, this.point));
		}

		[Test]
		public void TestEquals()
		{
			Assert.IsTrue(this.point.Equals(this.point));
			Assert.IsFalse(this.point.Equals(null));

			this.point.AbsoluteNumber = 1;
			this.point2.AbsoluteNumber = 2;

			Assert.IsFalse(this.point.Equals(this.point2));

			this.point2.AbsoluteNumber = 1;
			Assert.IsTrue(this.point.Equals(this.point2));
		}
	}
}
