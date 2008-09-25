//using System;
//using SharpCover.Actions;
//using NUnit.Framework;
//
//namespace SharpCover.Tasks
//{
//	[TestFixture]
//	public class SharpCoverTaskTests
//	{
//		public SharpCoverTaskTests()
//		{
//		}
//
//		private SharpCoverTask task;
//		private NAnt.Core.Types.FileSet files;
//		private SharpCoverActionMockObject mock;
//		[SetUp]
//		public void SetUp()
//		{
//			this.task = new SharpCoverTask();
//			this.mock = new SharpCoverActionMockObject();
//			this.task.Project = MockProject.GetMockProject();
//
//			files = new NAnt.Core.Types.FileSet();
//			files.FileNames.Add("foo.txt");
//			files.FileNames.Add("bar.txt");
//			this.task.Files = files;
//		}
//
//		[Test]
//		public void TestProperties()
//		{
//			Assert.AreEqual(Environment.CurrentDirectory, this.task.ReportDir);
//			Assert.AreEqual("Report", this.task.ReportName);
//
//			this.task.ReportDir = "c:\\foo\\bar\\";
//			this.task.ReportName = "bar";
//			this.task.Files = this.files;
//
//			Assert.AreEqual("c:\\foo\\bar\\", this.task.ReportDir);
//			Assert.AreEqual("bar", this.task.ReportName);
//			Assert.AreEqual(this.files, this.task.Files);
//
//			this.task.Action = this.mock;
//			Assert.AreEqual(this.mock, this.task.Action);
//		}
//
//		[Test]
//		[ExpectedException(typeof(NAnt.Core.ValidationException))]
//		public void TestExecute()
//		{			
//			this.task.Action = mock;
//
//			Assert.AreEqual(0, mock.NumExecutes);
//			this.task.Run();
//			Assert.AreEqual(1, mock.NumExecutes);
//			Assert.AreEqual(this.task.ReportName, mock.Settings.ReportName);
//			Assert.AreEqual(this.task.ReportDir, mock.Settings.ReportDir);
//			Assert.AreEqual(0, mock.Filenames.Count);
//		}
//	}
//}
