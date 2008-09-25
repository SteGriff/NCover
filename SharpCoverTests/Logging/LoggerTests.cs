using System;
using System.Diagnostics;
using NUnit.Framework;

namespace SharpCover.Logging
{
	[TestFixture]
	public class LoggerTests
	{
		public LoggerTests()
		{
		}

		[Test]
		public void TestInitializer()
		{
			Logger.OutputType.Level = TraceLevel.Info;
			Logger.AddConsoleListener("foo");
			Assert.AreEqual(TraceLevel.Info, Logger.OutputType.Level);
		
			Trace.Listeners.Remove("foo");
		}

		[Test]
		public void TestAddConsoleListener()
		{
			Assert.IsNull(Trace.Listeners["foo"]);
			Logger.AddConsoleListener("foo");
			Assert.IsNotNull(Trace.Listeners["foo"]);
			Assert.AreEqual(typeof(ConsoleTraceListener), Trace.Listeners["foo"].GetType());
		
			Trace.Listeners.Remove("foo");
		}

		[Test]
		public void TestAddNAntListener()
		{
			Assert.IsNull(Trace.Listeners["NAnt"]);

			Assert.AreEqual(TraceLevel.Info, Logger.OutputType.Level);
			SharpCoverTask task = new SharpCoverTask(); 
			NAntLogger.AddNAntListener(new SharpCoverTask());
			Assert.AreEqual(TraceLevel.Info, Logger.OutputType.Level);
			
			Assert.IsNotNull(Trace.Listeners["NAnt"]);

			Assert.AreEqual(typeof(NAntTraceListener), Trace.Listeners["NAnt"].GetType());

			task.Verbose = true;

			NAntLogger.AddNAntListener(task);
			Assert.AreEqual(TraceLevel.Verbose, Logger.OutputType.Level);
		
			Trace.Listeners.Remove("NAnt");
		}
	}
}