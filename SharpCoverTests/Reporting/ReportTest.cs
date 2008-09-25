using System;
using NUnit.Framework;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class ReportTest
	{
		public ReportTest()
		{
		}

		private Report report;

		[SetUp]
		public void SetUp()
		{
			this.report = new Report();
		}

		[Test]
		public void TestDefaults()
		{
			Assert.IsNotNull(this.report.Namespaces);
			Assert.AreEqual(0, this.report.Namespaces.Count);

			Assert.AreEqual(0, this.report.NumberOfPoints);
			Assert.AreEqual(0, this.report.NumberOfHitPoints);
			Assert.AreEqual(0, this.report.CoveragePercentage);

			try
			{
				string s = this.report.CoveragePercentageColor;
				Assert.Fail();
			}
			catch(InvalidOperationException)
			{
			}

			Assert.IsNull(this.report.History);
			Assert.IsNull(this.report.ReportName);
			Assert.IsNull(this.report.SourceDir);
			Assert.IsNull(this.report.ReportDate);
			Assert.IsNull(this.report.SharpCoverVersion);
		}
	}
}
