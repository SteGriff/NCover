using System;
using SharpCover.Reporting;
using SharpCover.Collections;
using NUnit.Framework;

namespace SharpCover.Collections
{
	[TestFixture]
	public class ReportFileCollectionTest
	{
		public ReportFileCollectionTest()
		{
		}

		private ReportFileCollection files;
		private ReportFile file;

		[SetUp]
		public void Setup()
		{
			this.files = new ReportFileCollection();
			this.file = new ReportFile();
		}

		[Test]
		public void TestCount()
		{
			Assert.AreEqual(0, this.files.Count);
			this.files.Add(this.file);
			Assert.AreEqual(1, this.files.Count);
			this.files.Add(this.file);
			Assert.AreEqual(2, this.files.Count);
		}

		[Test]
		public void TestAdd()
		{
			int index = this.files.Add(this.file);
			Assert.AreEqual(0, index);
			
			index = this.files.Add(this.file);
			Assert.AreEqual(1, index);

			index = this.files.Add(this.file);
			Assert.AreEqual(2, index);

			index = this.files.Add(this.file);
			Assert.AreEqual(3, index);
		}

		[Test]
		public void TestIndexer()
		{
			Assert.AreEqual(0, this.files.Count);
			Assert.AreEqual(null, this.files[this.files.Count], "Indexer should return null if indexout of bounds");
			Assert.AreEqual(null, this.files[-1], "Indexer should return null if indexout of bounds");

			this.files.Add(this.file);
			Assert.AreEqual(this.file, this.files[0]);
		}

		[Test]
		public void Remove()
		{
			Assert.AreEqual(0, this.files.Count);
			this.files.Add(this.file);
			Assert.AreEqual(1, this.files.Count);
			this.files.Remove(this.file);
			Assert.AreEqual(0, this.files.Count);
		}

		[Test]
		public void Insert()
		{
			this.files.Add(this.file);
			this.files.Add(this.file);
			this.files.Add(this.file);
			this.files.Add(this.file);

			ReportFile file1 = new ReportFile();
			this.files.Insert(2, file1);

			Assert.AreEqual(file1, this.files[2]);
		}

		[Test]
		public void Contains()
		{
			Assert.IsFalse(this.files.Contains(this.file));
			Assert.IsFalse(this.files.Contains(this.file.Filename));

			this.files.Add(this.file);

			Assert.IsTrue(this.files.Contains(this.file));
			Assert.IsTrue(this.files.Contains(this.file.Filename));
		}
	}
}