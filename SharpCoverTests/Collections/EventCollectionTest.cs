using NUnit.Framework;
using SharpCover.Reporting;

namespace SharpCover.Collections
{
	[TestFixture]
	public class EventCollectionTest
	{
		public EventCollectionTest()
		{
		}

		private EventCollection events;
		private Event evnt;

		[SetUp]
		public void Setup()
		{
			this.events = new EventCollection();
			this.evnt = new Event();
		}

		[Test]
		public void TestCount()
		{
			Assert.AreEqual(0, this.events.Count);
			this.events.Add(this.evnt);
			Assert.AreEqual(1, this.events.Count);
			this.events.Add(this.evnt);
			Assert.AreEqual(2, this.events.Count);
		}

		[Test]
		public void TestAdd()
		{
			int index = this.events.Add(this.evnt);
			Assert.AreEqual(0, index);
			
			index = this.events.Add(this.evnt);
			Assert.AreEqual(1, index);

			index = this.events.Add(this.evnt);
			Assert.AreEqual(2, index);

			index = this.events.Add(this.evnt);
			Assert.AreEqual(3, index);
		}

		[Test]
		public void TestIndexer()
		{
			Assert.AreEqual(0, this.events.Count);
			Assert.AreEqual(null, this.events[this.events.Count], "Indexer should return null if indexout of bounds");
			Assert.AreEqual(null, this.events[-1], "Indexer should return null if indexout of bounds");

			this.events.Add(this.evnt);
			Assert.AreEqual(this.evnt, this.events[0]);
		}

		[Test]
		public void Remove()
		{
			Assert.AreEqual(0, this.events.Count);
			this.events.Add(this.evnt);
			Assert.AreEqual(1, this.events.Count);
			this.events.Remove(this.evnt);
			Assert.AreEqual(0, this.events.Count);
		}

		[Test]
		public void Insert()
		{
			this.events.Add(this.evnt);
			this.events.Add(this.evnt);
			this.events.Add(this.evnt);
			this.events.Add(this.evnt);

			Event e1 = new Event();
			this.events.Insert(2, e1);

			Assert.AreEqual(e1, this.events[2]);
		}

	}
}
