using System;
using System.IO;
using NUnit.Framework;

namespace SharpCover.Logging
{
	[TestFixture]
	public class ConsoleTraceListenerTests
	{
		public ConsoleTraceListenerTests()
		{
		}

		[Test]
		public void TestConstructors()
		{
			ConsoleTraceListener listener;

			listener = new ConsoleTraceListener();
			Assert.AreEqual("Console", listener.Name);

			listener = new ConsoleTraceListener("foo");
			Assert.AreEqual("foo", listener.Name);
		}

		[Test]
		public void TestFeatures()
		{
			MockWriter writer = new MockWriter();
			TextWriter oldwriter = Console.Out;
			ConsoleTraceListener listener = new ConsoleTraceListener();

			try
			{
				Console.SetOut(writer);
				listener.Write(1);
				Assert.AreEqual("1", writer.LastWrite);

				listener.Write(2, "integer");
				Assert.AreEqual("integer:\t2", writer.LastWrite);

				listener.WriteLine(3);
				Assert.AreEqual("3" + Environment.NewLine, writer.LastWrite);

				listener.WriteLine(4, "integer");
				Assert.AreEqual("integer:\t4" + Environment.NewLine, writer.LastWrite);

				listener.Flush();
				Assert.IsTrue(writer.FlushCalled);

				listener.Fail("foo", "bar");
				Assert.AreEqual("bar" + Environment.NewLine, writer.LastWrite);
			}
			finally
			{
				Console.SetOut(oldwriter);
			}
		}
	}
}
