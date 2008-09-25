using System.IO;
using NUnit.Framework;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class HtmlReportTests
	{
		[Test]
		public void TestReport()
		{
			ReportSettings settings = new ReportSettings();
			settings.ReportDir = Path.GetTempPath();
			settings.ReportName = "Test";

			Stream expected = ResourceManager.GetResource("SharpCover.Resources.ExpectedCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream actual = ResourceManager.GetResource("SharpCover.Resources.ActualCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream fixedfile = Coverage.FixActualFile(actual);

			Coverage result = Coverage.LoadCoverage(expected, fixedfile);


			ReportGenerator generator = new ReportGenerator();
			Report report = generator.GenerateReport(result);
			
			DeleteFiles(settings);
			CheckFilesNotExist(settings);

			SetGradient();

			try
			{
				HtmlReport.Generate(settings, report);
				CheckFilesExist(settings);			
			}
			finally
			{
				DeleteFiles(settings);
				Gradient.GetInstance().Points.Clear();
			}
		}

		private void CheckFilesNotExist(ReportSettings settings)
		{
			Assert.IsFalse(File.Exists(settings.ReportFilename));
			Assert.IsFalse(File.Exists(settings.CssFilename));
			Assert.IsFalse(File.Exists(settings.GetFilename("SharpCover", ".gif")));
		}

		private void CheckFilesExist(ReportSettings settings)
		{
			Assert.IsTrue(File.Exists(settings.ReportFilename));
			Assert.IsTrue(File.Exists(settings.CssFilename));
			Assert.IsTrue(File.Exists(settings.GetFilename("SharpCover", ".gif")));

			Assert.IsTrue(new FileInfo(settings.ReportFilename).Length > 0);
			Assert.IsTrue(new FileInfo(settings.CssFilename).Length > 0);
			Assert.IsTrue(new FileInfo(settings.GetFilename("SharpCover", ".gif")).Length > 0);
		}

		private void SetGradient()
		{
			Gradient gradient = Gradient.GetInstance();
			gradient.Points.Clear();
			gradient.Add(new GradientPoint(0, 255, 0, 0));
			gradient.Add(new GradientPoint(75, 200, 200, 0));
			gradient.Add(new GradientPoint(100, 0, 200, 0));
		}

		private void DeleteFiles(ReportSettings settings)
		{
			File.Delete(settings.ReportFilename);
			File.Delete(settings.CssFilename);
			File.Delete(settings.GetFilename("SharpCover", ".gif"));
		}
	}
}
