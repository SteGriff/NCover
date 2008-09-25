using System.Collections;
using System.IO;
using System.Collections.Specialized;
using NUnit.Framework;
using SharpCover;
using SharpCover.Instrumenting;
using SharpCover.Parsing;
using SharpCover.Parsing.CSharp;

namespace SharpCover.Actions
{
	[TestFixture]
	public class SharpCoverActionTests
	{
		public SharpCoverActionTests()
		{
		}

		private SharpCoverAction action;

		[SetUp]
		public void SetUp()
		{
			this.action = new SharpCoverAction();
		}

		[Test]
		public void TestProperties()
		{
			Assert.IsNotNull(this.action.Filenames);
			Assert.IsNotNull(this.action.Settings);
			Assert.IsNotNull(this.action.Instrumenter);

			StringCollection sc = new StringCollection();
			this.action.Filenames = sc;

			ReportSettings rs = new ReportSettings();
			this.action.Settings = rs;

			ArrayList parsers = new ArrayList();
			parsers.Add(new Parser(rs));
			Instrumenter instrumenter = new FileCopyInstrumenter(parsers);
			this.action.Instrumenter = instrumenter;

			Assert.AreEqual(sc, this.action.Filenames);
			Assert.AreEqual(rs, this.action.Settings);
			Assert.AreEqual(instrumenter, this.action.Instrumenter);
		}

		[Test]
		public void TestExecute()
		{
			ArrayList parsers = new ArrayList();
			parsers.Add(new MockParser());
			MockInstrumenter mi = new MockInstrumenter(parsers);
			this.action.Instrumenter = mi;
			this.action.Settings.ReportDir = Path.GetTempPath();
			this.action.Filenames.Add("foo.txt");

			try
			{
				int pointcount = (int)this.action.Execute();

				Assert.AreEqual(1, pointcount);
				Assert.IsTrue(File.Exists(this.action.Settings.ExpectedFilename));
			}
			finally
			{
				File.Delete(this.action.Settings.ExpectedFilename);
			}
		}
	}
}