using System.IO;
using NUnit.Framework;
using SharpCover.Utilities;

namespace SharpCover
{
	[TestFixture]
	public class CoverageTests
	{
		public CoverageTests()
		{
		}

		[Test]
		public void TestConstructors()
		{
			Coverage coverage;

			coverage = new Coverage();
			Assert.IsNotNull(coverage.Settings);

			ReportSettings settings = new ReportSettings();
			CoveragePoint[] points = new CoveragePoint[0];

			coverage = new Coverage(settings, points);
			Assert.AreEqual(settings, coverage.Settings);
			Assert.AreEqual(points, coverage.CoveragePoints);
		}

		[Test]
		public void TestProperties()
		{
			Coverage coverage = new Coverage();
			ReportSettings settings = new ReportSettings();
			coverage.Settings = settings;

			Assert.AreEqual(settings, coverage.Settings);
		}

		[Test]
		public void TestSave()
		{
			MemoryStream ms = new MemoryStream();
			Coverage coverage = new Coverage();

			Coverage.SaveCoverage(ms, coverage);

			Assert.IsFalse(ms.CanRead);
		}

		[Test]
		public void TestFixActualFile()
		{
			Stream actual = ResourceManager.GetResource("SharpCover.Resources.ActualCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream fixedfile = Coverage.FixActualFile(actual);
			Coverage coverage = (Coverage)Serialization.FromXml(fixedfile, typeof(Coverage));
			Assert.AreEqual(238, coverage.CoveragePoints.Length);
		}

		[Test]
		public void TestLoadCoverage()
		{
			Stream expected = ResourceManager.GetResource("SharpCover.Resources.ExpectedCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream actual = ResourceManager.GetResource("SharpCover.Resources.ActualCoverageFile.xml", typeof(CoverageTests).Assembly);
			Stream fixedfile = Coverage.FixActualFile(actual);

			Coverage result = Coverage.LoadCoverage(expected, fixedfile);

			Assert.AreEqual(354, result.CoveragePoints.Length);

			int i = 0;
			foreach(CoveragePoint point in result.CoveragePoints)
			{
				if(point.Hit)
					i++;
			}

			Assert.AreEqual(238, i);
		}
	}
}
