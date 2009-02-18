using System.IO;
using NUnit.Framework;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class HistoryManagerTest
	{
		public HistoryManagerTest()
		{
		}

		private History history;

		[SetUp]
		public void SetUp()
		{
			this.history = new History();
			this.history.Filename = Path.GetTempFileName();
			this.history.Events.Add(new Event(0.2M));

			Gradient.GetInstance().Points.Clear();
			Gradient.GetInstance().Add(new GradientPoint(100,255,0,0));
			Gradient.GetInstance().Add(new GradientPoint(0, 0, 255, 0));
		}

		[TearDown]
		public void TearDown()
		{
			Gradient.GetInstance().Points.Clear();
		}


		[Test]
		public void TestLoadSave()
		{
			HistoryManager.SaveHistory(this.history);
			if(!File.Exists(this.history.Filename) || new FileInfo(this.history.Filename).Length == 0)
				Assert.Fail("Failed to write history file");

			History history = HistoryManager.LoadHistory(this.history.Filename);
			Assert.AreEqual(1, history.Events.Count);
		}

		[Test]
		public void TestLoad()
		{
			Assert.IsNull(HistoryManager.LoadHistory(null));
			
			Assert.IsFalse(File.Exists("foo.bar"));
			History hist = HistoryManager.LoadHistory("foo.bar");
			Assert.AreEqual("foo.bar", hist.Filename);
			Assert.AreEqual(0, hist.Events.Count);
		}

		[Test]
		public void TestUpdate()
		{
			Assert.AreEqual(1, this.history.Events.Count);
			Assert.AreEqual(0.2M, this.history.GetLastEvent().CoveragePercentage);

			Report report = new Report();

			HistoryManager.Update(this.history, report);
			Assert.AreEqual(2, this.history.Events.Count);
			Assert.AreEqual(0, this.history.GetLastEvent().CoveragePercentage);
		}
	}
}
