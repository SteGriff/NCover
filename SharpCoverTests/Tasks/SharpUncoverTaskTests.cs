//using System;
//using SharpCover.Actions;
//using NUnit.Framework;
//
//namespace SharpCover.Tasks
//{
//	[TestFixture]
//	public class SharpUncoverTaskTests
//	{
//		public SharpUncoverTaskTests()
//		{
//		}
//
//		private SharpUncoverTask task;
//		private SharpCoverActionMockObject mock;
//
//		[SetUp]
//		public void SetUp()
//		{
//			this.mock  = new SharpCoverActionMockObject();
//			this.task = new SharpUncoverTask();
//			this.task.Project = MockProject.GetMockProject();
//		}
//
//		[Test]
//		public void TestProperties()
//		{
//			Assert.AreEqual(Environment.CurrentDirectory, this.task.ReportDir);
//			Assert.AreEqual("Report", this.task.ReportName);
//
//			this.task.ReportDir = "c:\foo";
//			this.task.ReportName = "bar";
//
//			Assert.AreEqual("c:\foo", this.task.ReportDir);
//			Assert.AreEqual("bar", this.task.ReportName);
//
//			this.task.Action = this.mock;
//			Assert.AreEqual(this.mock, this.task.Action);
//		}
//
//		[Test]
//		public void TestExecute()
//		{			
//			this.task.Action = mock;
//
//			Assert.AreEqual(0, mock.NumExecutes);
//			this.task.Run();
//			Assert.AreEqual(1, mock.NumExecutes);
//			Assert.AreEqual(this.task.ReportName, mock.Settings.ReportName);
//			Assert.AreEqual(this.task.ReportDir, mock.Settings.ReportDir);
//		}
//	}
//}
