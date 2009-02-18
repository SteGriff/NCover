using System.IO;
using NUnit.Framework;

namespace SharpCover.Utilities
{
	[TestFixture]
	public class SerializationTest
	{
		public SerializationTest()
		{
		}

		[Test]
		public void TestStreamClosed()
		{
			object obj = new object();
			MemoryStream ms = new MemoryStream();
			Assert.IsTrue(ms.CanRead);

			Serialization.ToXml(ms, obj, true);

			Assert.IsFalse(ms.CanRead);
		}

		[Test]
		public void TestStreamNotClosed()
		{
			object obj = new object();
			MemoryStream ms = new MemoryStream();
			Assert.IsTrue(ms.CanRead);

			Serialization.ToXml(ms, obj, false);

			Assert.IsTrue(ms.CanRead);
		}


		[Test]
		public void TestSerializationDeSerialization()
		{
			TestObject to = new TestObject();
			to.One = 1;
			to.Two = 2;

			MemoryStream ms = new MemoryStream();
			Serialization.ToXml(ms, to, false);
			ms.Seek(0,0);

			TestObject to1 = new TestObject();
			to1 = (TestObject)Serialization.FromXml(ms, to.GetType());

			Assert.AreEqual(to.One, to1.One);
			Assert.AreEqual(to.Two, to1.Two);
		}
	}

	public class TestObject
	{
		public TestObject()
		{
		}

		private int one = 0;
		private int two = 0;
		
		public int One
		{
			get{return this.one;}
			set{this.one = value;}
		}

		public int Two
		{
			get{return this.two;}
			set{this.two = value;}
		}
	}
}
