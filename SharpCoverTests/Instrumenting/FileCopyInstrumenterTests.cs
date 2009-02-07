using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SharpCover.Parsing;

namespace SharpCover.Instrumenting
{
	[TestFixture]
	public class FileCopyInstrumenterTests
	{
		public FileCopyInstrumenterTests()
		{
		}

		private FileCopyInstrumenter instrumenter;
		private MockParser parser;

		[SetUp]
		public void SetUp()
		{
			this.parser = new MockParser();
            List<IParse> parsers = new List<IParse>(1);
			parsers.Add(parser);
			this.instrumenter = new FileCopyInstrumenter(parsers.ToArray());
		}

		[Test]
		public void TestInstrumentWithNoPointsReturned()
		{
			CoveragePoint[] points = this.instrumenter.Instrument("foo");
			Assert.AreEqual(0, points.Length);
		}

		[Test]
		public void TestInstrumentWithPointsReturned()
		{
			CoveragePoint[] parserpoints = new CoveragePoint[1];
			parserpoints[0] = new CoveragePoint();
			this.parser.CoveragePoints = parserpoints;

			string filename = Path.GetTempFileName();
			Assert.IsTrue(File.Exists(filename));

			string instrumentedfilename = filename + ".instrumented";
			Assert.IsFalse(File.Exists(instrumentedfilename));

			CoveragePoint[] points = this.instrumenter.Instrument(filename);
			Assert.AreEqual(1, points.Length);

			Assert.IsTrue(File.Exists(filename));
			Assert.IsTrue(File.Exists(instrumentedfilename));

			using(StreamReader stream = File.OpenText(filename))
			{
				Assert.AreEqual(MockParser.PARSED_TEXT + "\r\n", stream.ReadToEnd());
			}

			using(StreamReader stream = File.OpenText(instrumentedfilename ))
			{
				Assert.IsFalse((MockParser.PARSED_TEXT + "\r\n").Equals( stream.ReadToEnd()));
			}

			points = this.instrumenter.Instrument(filename);

			Assert.IsTrue(File.Exists(filename));
			Assert.IsTrue(File.Exists(instrumentedfilename));

			using(StreamReader stream = File.OpenText(instrumentedfilename ))
			{
				Assert.IsFalse((MockParser.PARSED_TEXT + "\r\n").Equals( stream.ReadToEnd()));
			}

			this.instrumenter.Deinstrument(filename);
			Assert.IsFalse(File.Exists(instrumentedfilename));
		}
	}
}
