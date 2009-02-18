using NUnit.Framework;
using SharpCover.Reporting;

namespace SharpCover.Collections
{
	[TestFixture]
	public class NamespaceCollectionTest
	{
		public NamespaceCollectionTest()
		{
		}

		private NamespaceCollection namespaces;
		private Namespace ns;

		[SetUp]
		public void Setup()
		{
			this.namespaces = new NamespaceCollection();
			this.ns = new Namespace();
		}

		[Test]
		public void TestCount()
		{
			Assert.AreEqual(0, this.namespaces.Count);
			this.namespaces.Add(this.ns);
			Assert.AreEqual(1, this.namespaces.Count);
			this.namespaces.Add(this.ns);
			Assert.AreEqual(2, this.namespaces.Count);
		}

		[Test]
		public void TestAdd()
		{
			int index = this.namespaces.Add(this.ns);
			Assert.AreEqual(0, index);
			
			index = this.namespaces.Add(this.ns);
			Assert.AreEqual(1, index);

			index = this.namespaces.Add(this.ns);
			Assert.AreEqual(2, index);

			index = this.namespaces.Add(this.ns);
			Assert.AreEqual(3, index);
		}

		[Test]
		public void TestIndexer()
		{
			Assert.AreEqual(0, this.namespaces.Count);
			Assert.AreEqual(null, this.namespaces[this.namespaces.Count], "Indexer should return null if indexout of bounds");
			Assert.AreEqual(null, this.namespaces[-1], "Indexer should return null if indexout of bounds");

			this.namespaces.Add(this.ns);
			Assert.AreEqual(this.ns, this.namespaces[0]);
		}

		[Test]
		public void Remove()
		{
			Assert.AreEqual(0, this.namespaces.Count);
			this.namespaces.Add(this.ns);
			Assert.AreEqual(1, this.namespaces.Count);
			this.namespaces.Remove(this.ns);
			Assert.AreEqual(0, this.namespaces.Count);
		}

		[Test]
		public void Insert()
		{
			this.namespaces.Add(this.ns);
			this.namespaces.Add(this.ns);
			this.namespaces.Add(this.ns);
			this.namespaces.Add(this.ns);

			Namespace ns1 = new Namespace();
			this.namespaces.Insert(2, ns1);

			Assert.AreEqual(ns1, this.namespaces[2]);
		}

		[Test]
		public void Contains()
		{
			Assert.IsFalse(this.namespaces.Contains(this.ns));
			Assert.IsFalse(this.namespaces.Contains(this.ns.Name));

			this.namespaces.Add(this.ns);

			Assert.IsTrue(this.namespaces.Contains(this.ns));
			Assert.IsTrue(this.namespaces.Contains(this.ns.Name));
		}
	}
}
