using NUnit.Framework;

namespace SharpCover
{
	[TestFixture]
	public class ReportSettingsTest
	{
		public ReportSettingsTest()
		{
		}

		private ReportSettings settings;

		[SetUp]
		public void Setup()
		{
			this.settings = new ReportSettings();
		
			this.settings.BaseDir = "c:\\foo\\";
			this.settings.ReportName = "Test";
			this.settings.ReportDir = "bar";
		}

		[Test]
		public void TestExpectedFilename()
		{
			string filename = "c:\\foo\\bar\\Test-expected.xml";
			Assert.AreEqual(filename, this.settings.ExpectedFilename);

			this.settings.ReportDir = "c:\\foo\\bar\\";
			Assert.AreEqual(filename, this.settings.ExpectedFilename);
		}

		[Test]
		public void TestActualFilename()
		{
			string filename = "c:\\foo\\bar\\Test-actual.xml";
			Assert.AreEqual(filename, this.settings.ActualFilename);

			this.settings.ReportDir = "c:\\foo\\bar\\";
			Assert.AreEqual(filename, this.settings.ActualFilename);
		}

		[Test]
		public void TestHistoryFilename()
		{
			string filename = "c:\\foo\\bar\\Test-history.xml";
			Assert.AreEqual(filename, this.settings.HistoryFilename);

			this.settings.ReportDir = "c:\\foo\\bar\\";
			Assert.AreEqual(filename, this.settings.HistoryFilename);
		}

		[Test]
		public void TestCssFilename()
		{
			string filename = "c:\\foo\\bar\\sharpcover.css";
			Assert.AreEqual(filename, this.settings.CssFilename);

			this.settings.ReportDir = "c:\\foo\\bar\\";
			Assert.AreEqual(filename, this.settings.CssFilename);
		}

		[Test]
		public void TestReportFilename()
		{
			string filename = "c:\\foo\\bar\\Test-report.html";
			Assert.AreEqual(filename, this.settings.ReportFilename);

			this.settings.ReportDir = "c:\\foo\\bar\\";
			Assert.AreEqual(filename, this.settings.ReportFilename);
		}

		[Test]
		public void TestMinimumCoverage()
		{
			this.settings.MinimumCoverage = 0.5M;
			Assert.AreEqual(0.5, this.settings.MinimumCoverage);

			this.settings.MinimumCoverage = 50;
			Assert.AreEqual(0.5, this.settings.MinimumCoverage);
		}
	}
}