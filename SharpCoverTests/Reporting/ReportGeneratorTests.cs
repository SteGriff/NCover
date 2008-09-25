using System;
using System.IO;
using NUnit.Framework;
using SharpCover;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class ReportGeneratorTests
	{
		public ReportGeneratorTests()
		{
		}

		private ReportGenerator generator;

		[SetUp]
		public void SetUp()
		{
			this.generator = new ReportGenerator();
		}

		[Test]
		public void GenerateReportFromResources()
		{
			Stream expected = ResourceManager.GetResource("SharpCover.Resources.ExpectedCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream actual = ResourceManager.GetResource("SharpCover.Resources.ActualCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream fixedfile = Coverage.FixActualFile(actual);

			Coverage result = Coverage.LoadCoverage(expected, fixedfile);

			Report report = this.generator.GenerateReport(result);

			Assert.AreEqual(8, report.Namespaces.Count);
			Assert.AreEqual(67, (int)(report.CoveragePercentage * 100));
			Assert.AreEqual(238, report.NumberOfHitPoints);
			Assert.AreEqual(354, report.NumberOfPoints);

			Namespace ns = report.Namespaces[0];
			Assert.AreEqual("SharpCover.Actions", ns.Name);
			Assert.AreEqual(3, ns.Files.Count);

			Assert.AreEqual(result.Settings.ReportName, report.ReportName);
			Assert.AreEqual(result.Settings.BaseDir, report.SourceDir);
			Assert.AreEqual(result.GetType().Assembly.GetName().Version.ToString(), report.SharpCoverVersion);
		}
	}
}
