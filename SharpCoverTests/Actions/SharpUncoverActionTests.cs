using System.Collections;
using System.IO;
using NUnit.Framework;
using SharpCover;
using SharpCover.Instrumenting;
using SharpCover.Parsing;
using SharpCover.Utilities;

namespace SharpCover.Actions
{
	[TestFixture]
	public class SharpUncoverActionTests
	{
		public SharpUncoverActionTests()
		{
		}

		private SharpUncoverAction action;

		[SetUp]
		public void SetUp()
		{
			this.action = new SharpUncoverAction();
		}

		[Test]
		public void TestProperties()
		{
			Assert.IsNotNull(this.action.Filenames);
			Assert.IsNotNull(this.action.Settings);

			ReportSettings rs = new ReportSettings();
			this.action.Settings = rs;

			Instrumenter instrumenter = new FileCopyInstrumenter(null);
			this.action.Instrumenter = instrumenter;

			Assert.AreEqual(rs, this.action.Settings);
			Assert.AreEqual(instrumenter, this.action.Instrumenter);
		}

		[Test]
		public void TestExceptions()
		{
			decimal retval = this.action.Execute();
			Assert.AreEqual(-1, retval);
		}

		[Test]
		public void TestExecute()
		{
			ArrayList parsers = new ArrayList();
			parsers.Add(new MockParser());
			MockInstrumenter mi = new MockInstrumenter(parsers);
			this.action.Instrumenter = mi;
			this.action.Settings.ReportDir = Path.GetTempPath();
			
			Assert.IsFalse(File.Exists(this.action.Settings.ExpectedFilename));
			int retval = (int)this.action.Execute();
			Assert.AreEqual(-1, retval);

			try
			{
				FileStream fs = new FileStream(this.action.Settings.ExpectedFilename, FileMode.Create);
				ResourceManager.WriteResourceToStream(fs, "SharpCover.Resources.ExpectedCoverageFile.xml", ResourceType.Text, this.GetType().Assembly);
				fs.Close();

				Assert.IsTrue(File.Exists(this.action.Settings.ExpectedFilename));

				retval = (int)this.action.Execute();
				Assert.IsTrue(mi.DeinstrumentCalled);
				Assert.AreEqual(0, retval);
			}
			finally
			{
				File.Delete(this.action.Settings.ExpectedFilename);
			}
		}
	}
}