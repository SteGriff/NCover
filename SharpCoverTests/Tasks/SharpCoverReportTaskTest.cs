//using System;
//using System.Diagnostics;
//using SharpCover.Logging;
//using SharpCover.Actions;
//using NUnit.Framework;
//
//namespace SharpCover.Tasks
//{
//	[TestFixture]
//	public class SharpCoverReportTaskTests
//	{
//		public SharpCoverReportTaskTests()
//		{
//		}
//
//		private SharpCoverReportTask task;
//		private SharpCoverActionMockObject mock;
//		private MockTraceListener listener;
//
//		[SetUp]
//		public void SetUp()
//		{
//			this.task = new SharpCoverReportTask();
//			this.task.Project = MockProject.GetMockProject();
//			this.mock = new SharpCoverActionMockObject();
//			this.task.Action = this.mock;
//
//			this.listener = new MockTraceListener();
//			Trace.Listeners.Add(this.listener);
//		}
//
//		[TearDown]
//		public void TearDown()
//		{
//			Trace.Listeners.Remove(this.listener);
//		}
//
//		[Test]
//		public void TestProperties()
//		{
//			Assert.AreEqual(Environment.CurrentDirectory, this.task.ReportDir);
//			Assert.AreEqual("Report", this.task.ReportName);
//			Assert.AreEqual(0, this.task.Minimum);
//
//			this.task.ReportDir = "c:\foo";
//			this.task.ReportName = "bar";
//			this.task.Minimum = 0.25M;
//			this.task.Action = this.mock;
//
//			Assert.AreEqual(0.25M, this.task.Minimum);
//			Assert.AreEqual("c:\foo", this.task.ReportDir);
//			Assert.AreEqual("bar", this.task.ReportName);
//			Assert.AreEqual(this.mock, this.task.Action);
//		}
//
//		[Test]
//		public void TestNormalExecute()
//		{
//			Execute(0, 0.2M);	
//		}
//
//		[Test]
//		[ExpectedException(typeof(NAnt.Core.BuildException))]
//		public void TestTooLowExecute()
//		{
//			Execute(0.3M,0.2M);
//		}
//
//		[Test]
//		public void TestWithin10PercentExecute()
//		{
//			Logger.OutputType.Level = TraceLevel.Verbose;
//			int orig = this.listener.NumWriteLines;
//			Execute(0.3M,0.35M);
//			Assert.AreEqual(orig + 2, this.listener.NumWriteLines);
//		}
//
//		private void Execute(decimal minimum, decimal retval)
//		{
//			this.task.Minimum = minimum;
//			this.mock.ValueToReturn = retval;
//
//			Assert.AreEqual(0, this.mock.NumExecutes);
//			this.task.Run();
//			Assert.AreEqual(1, this.mock.NumExecutes);
//			Assert.AreEqual(this.task.ReportName, this.mock.Settings.ReportName);
//			Assert.AreEqual(this.task.ReportDir, this.mock.Settings.ReportDir);
//		}
//	}
//}
