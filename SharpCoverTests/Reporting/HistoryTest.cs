using System;
using NUnit.Framework;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class HistoryTest
	{
		public HistoryTest()
		{
		}

		private History history;
		private Event event1;
		private Event event2;
		private Event event3;

		[SetUp]
		public void SetUp()
		{
			this.history = new History();

			this.event1 = new Event();
			this.event1.EventDate = new DateTime(2000,01,01);

			this.event2 = new Event();
			this.event2.EventDate = new DateTime(2001,01,01);
		
			this.event3 = new Event();
			this.event3.EventDate = new DateTime(2002,01,01);
		}

		[Test]
		public void TestConstructor()
		{
			Assert.IsNotNull(this.history.Events);

			History h = new History("foo.txt");
			Assert.AreEqual("foo.txt", h.Filename);
		}

		[Test]
		public void GetLastEvent()
		{
			Assert.AreEqual(0, this.history.Events.Count);
			Assert.AreEqual(null, this.history.GetLastEvent());

			this.history.Events.Add(this.event1);
			Assert.AreEqual(this.event1, this.history.GetLastEvent());

			this.history.Events.Add(this.event3);
			Assert.AreEqual(this.event3, this.history.GetLastEvent());

			this.history.Events.Add(this.event2);
			Assert.AreEqual(this.event3, this.history.GetLastEvent());
		}

		[Test]
		public void TestProperties()
		{
			Assert.AreEqual("", this.history.Filename);
			this.history.Filename = "foo.txt";
			Assert.AreEqual("foo.txt", this.history.Filename);
		}
	}
}
