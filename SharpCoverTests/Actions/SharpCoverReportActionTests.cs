using System.IO;
using NUnit.Framework;
using SharpCover.Utilities;

namespace SharpCover.Actions
{
	[TestFixture]
	public class SharpCoverReportActionTests
	{
		private SharpCoverReportAction action;

		[SetUp]
		public void SetUp()
		{
			this.action = new SharpCoverReportAction();
		}

		[Test]
		public void TestProperties()
		{
			Assert.IsNotNull(this.action.Filenames);
			Assert.IsNotNull(this.action.Settings);
		}

		[Test]
		public void TestExceptions()
		{
			decimal retval = this.action.Execute();
			Assert.AreEqual(0, retval);
		}

		[Test]
		public void TestExecute()
		{
			this.action.Settings.ReportDir = Path.GetTempPath();
			
			FileStream actualfs = new FileStream(this.action.Settings.ActualFilename, FileMode.Create);
			FileStream expectedfs = new FileStream(this.action.Settings.ExpectedFilename, FileMode.Create);

			ResourceManager.WriteResourceToStream(actualfs, "SharpCover.Resources.ActualCoverageFile.xml", ResourceType.Text, this.GetType().Assembly);
			ResourceManager.WriteResourceToStream(expectedfs, "SharpCover.Resources.ExpectedCoverageFile.xml", ResourceType.Text, this.GetType().Assembly);
		
			actualfs.Close();
			expectedfs.Close();

			try
			{
				int retval = (int)(this.action.Execute() * 100);

				Assert.AreEqual(67, retval);
			}
			finally
			{
				File.Delete(this.action.Settings.ExpectedFilename);
				File.Delete(this.action.Settings.ActualFilename);
				File.Delete(this.action.Settings.HistoryFilename);
				File.Delete(this.action.Settings.ReportFilename);
			}
		}

	}
}