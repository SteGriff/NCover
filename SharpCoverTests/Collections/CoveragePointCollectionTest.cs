using NUnit.Framework;

namespace SharpCover.Collections
{
	[TestFixture]
	public class CoveragePointCollectionTest
	{
		public CoveragePointCollectionTest()
		{
		}

		private CoveragePointCollection points;
		private CoveragePoint point;
		private CoveragePoint point1;
		private CoveragePoint point2;

		[SetUp]
		public void Setup()
		{
			this.points = new CoveragePointCollection();
			
			this.point = new CoveragePoint();
			this.point.Hit = true;
			this.point.LineNumber = 10;
		
			this.point1 = new CoveragePoint();
			this.point1.Hit = false;
			this.point1.LineNumber = 20;

			this.point2 = new CoveragePoint();
			this.point2.Hit = false;
			this.point2.LineNumber = 30;
		}

		[TearDown]
		public void TearDown()
		{
			this.points.Clear();
		}

		[Test]
		public void TestCount()
		{
			Assert.AreEqual(0, this.points.Count);
			this.points.Add(this.point);
			Assert.AreEqual(1, this.points.Count);
			this.points.Add(this.point);
			Assert.AreEqual(2, this.points.Count);
		}

		[Test]
		public void TestAdd()
		{
			int index = this.points.Add(this.point);
			Assert.AreEqual(0, index);
			
			index = this.points.Add(this.point);
			Assert.AreEqual(1, index);

			index = this.points.Add(this.point);
			Assert.AreEqual(2, index);

			index = this.points.Add(this.point);
			Assert.AreEqual(3, index);
		}

		[Test]
		public void TestIndexer()
		{
			Assert.AreEqual(0, this.points.Count);
			Assert.AreEqual(null, this.points[this.points.Count], "Indexer should return null if indexout of bounds");
			Assert.AreEqual(null, this.points[-1], "Indexer should return null if indexout of bounds");

			this.points.Add(this.point);
			Assert.AreEqual(this.point, this.points[0]);
		}

		[Test]
		public void Remove()
		{
			Assert.AreEqual(0, this.points.Count);
			this.points.Add(this.point);
			Assert.AreEqual(1, this.points.Count);
			this.points.Remove(this.point);
			Assert.AreEqual(0, this.points.Count);
		}

		[Test]
		public void Insert()
		{
			this.points.Add(this.point);
			this.points.Add(this.point);
			this.points.Add(this.point);
			this.points.Add(this.point);

			this.points.Insert(2, this.point1);

			Assert.AreEqual(this.point1, this.points[2]);
		}

		[Test]
		public void MissedLineNumbers()
		{
			Assert.IsTrue(this.point.Hit);
			Assert.IsFalse(this.point1.Hit);
			Assert.IsFalse(this.point2.Hit);

			Assert.AreEqual("", this.points.MissedLineNumbers);
			
			this.points.Add(this.point);
			Assert.AreEqual("", this.points.MissedLineNumbers);

			this.points.Add(this.point1);
			Assert.AreEqual("20", this.points.MissedLineNumbers);

			this.points.Add(this.point2);
			Assert.AreEqual("20, 30", this.points.MissedLineNumbers);
		}

		[Test]
		public void NumberOfPoints()
		{
			Assert.AreEqual(0, this.points.NumberOfPoints);
			
			this.points.Add(this.point);
			Assert.AreEqual(1, this.points.NumberOfPoints);

			this.points.Add(this.point1);
			Assert.AreEqual(2, this.points.NumberOfPoints);

			this.points.Add(this.point2);
			Assert.AreEqual(3, this.points.NumberOfPoints);
		}

		[Test]
		public void NumberOfHitPoints()
		{
			Assert.IsTrue(this.point.Hit);
			Assert.IsFalse(this.point1.Hit);
			Assert.IsFalse(this.point2.Hit);

			Assert.AreEqual(0, this.points.NumberOfHitPoints);
			
			this.points.Add(this.point);
			Assert.AreEqual(1, this.points.NumberOfHitPoints);

			this.points.Add(this.point1);
			Assert.AreEqual(1, this.points.NumberOfHitPoints);

			this.points.Add(this.point2);
			Assert.AreEqual(1, this.points.NumberOfHitPoints);
		}

		[Test]
		public void CoveragePercentage()
		{
			Assert.IsTrue(this.point.Hit);
			Assert.IsFalse(this.point1.Hit);
			Assert.IsFalse(this.point2.Hit);

			Assert.AreEqual(0, this.points.CoveragePercentage);

			this.points.Add(this.point);
			Assert.AreEqual(1, this.points.CoveragePercentage);
			
			this.points.Add(this.point1);
			Assert.AreEqual(1M/2M, this.points.CoveragePercentage);

			this.points.Add(this.point2);
			Assert.AreEqual(1M/3M, this.points.CoveragePercentage);
		}
	}
}