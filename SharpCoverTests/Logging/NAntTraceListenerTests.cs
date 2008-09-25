//using System;
//using System.Xml;
//using NAnt.Core;
//using NUnit.Framework;
//using SharpCover.Tasks;
//using SharpCover.Utilities;
//
//namespace SharpCover.Logging
//{
//	[TestFixture]
//	public class NAntTraceListenerTests
//	{
//		public NAntTraceListenerTests()
//		{
//		}
//
//		private MockTask task;
//
//		[SetUp]
//		public void SetUp()
//		{
//			this.task = new MockTask();
//		}
//
//		[Test]
//		public void TestConstructors()
//		{
//			NAntTraceListener listener = new NAntTraceListener(this.task);
//			Assert.AreEqual("NAnt", listener.Name);
//
//			listener = new NAntTraceListener(this.task, "foo");
//			Assert.AreEqual("foo", listener.Name);
//		}
//
//		[Test]
//		public void Test()
//		{
//			
//		}
//
//		[Test]
//		public void TestFeatures()
//		{
//			NAntTraceListener listener = new NAntTraceListener(this.task);
//
//				listener.Write(1);
//				Assert.AreEqual("1", this.task.LastWrite);
//
//				listener.Write(2, "integer");
//				Assert.AreEqual("integer:\t2", this.task.LastWrite);
//
//				listener.WriteLine(3);
//				Assert.AreEqual("3", this.task.LastWrite);
//
//				listener.WriteLine(4, "integer");
//				Assert.AreEqual("integer:\t4", this.task.LastWrite);
//
//				listener.Fail("foo", "bar");
//				Assert.AreEqual("bar", this.task.LastWrite);
//		}
//	}
//}
