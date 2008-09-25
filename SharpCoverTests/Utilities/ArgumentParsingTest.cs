using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace SharpCover.Utilities
{
	[TestFixture]
	public class ArgumentParsingTest
	{
		[Test]
		public void PositiveParses()
		{
			string key, value;
			ArgumentParsing.ParseArgument("/debug:true", out key, out value);
			Assert.AreEqual("debug", key);
			Assert.AreEqual("true", value);

			ArgumentParsing.ParseArgument("/debug:\"true\"", out key, out value);
			Assert.AreEqual("debug", key);
			Assert.AreEqual("true", value);
		
			ArgumentParsing.ParseArgument("/debug", out key, out value);
			Assert.AreEqual("debug", key);
			Assert.IsNull(value);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void BadArguments()
		{
			string key, value;
			ArgumentParsing.ParseArgument("/fdsfs:fdsf:fds", out key, out value);
		}

		[Test]
		public void MultipleArguments()
		{
			string[] args = new string[] { "/testarg:testvalue", "/foo:bar" };
			NameValueCollection nvc = ArgumentParsing.ParseCommandLine(args);
			Assert.AreEqual(2, nvc.Count, "Should be 2 arguments in the collection");

			nvc = ArgumentParsing.ParseCommandLine(null);
			Assert.AreEqual(0, nvc.Count, "Should be 0 arguments in the collection");

			nvc = ArgumentParsing.ParseCommandLine(new string[0]);
			Assert.AreEqual(0, nvc.Count, "Should be 0 arguments in the collection");
		}
	}
}
