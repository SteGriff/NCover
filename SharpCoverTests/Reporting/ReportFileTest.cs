using System;
using NUnit.Framework;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class ReportFileTest
	{
		public ReportFileTest()
		{
		}

		private ReportFile file;

		[SetUp]
		public void SetUp()
		{
			this.file = new ReportFile();
		}

		[Test]
		public void TestDefaults()
		{
			Assert.IsNotNull(this.file.Points);
			Assert.AreEqual(0, this.file.Points.Count);

			Assert.AreEqual(0, this.file.NumberOfPoints);
			Assert.AreEqual(0, this.file.NumberOfHitPoints);
			Assert.AreEqual(0, this.file.CoveragePercentage);

			try
			{
				string s = this.file.CoveragePercentageColor;
				Assert.Fail();
			}
			catch(InvalidOperationException)
			{
			}

			Assert.IsNull(this.file.Filename);
			Assert.AreEqual("", this.file.MissedLineNumbers);
			Assert.IsNull(this.file.Name);
		}
	}
}
