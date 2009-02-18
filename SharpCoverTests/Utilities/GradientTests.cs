using System.Drawing;
using NUnit.Framework;

namespace SharpCover.Utilities
{
	[TestFixture]
	public class GradientTests
	{
		public GradientTests()
		{
		}

		private Gradient gradient;
		private GradientPoint[] points;

		[SetUp]
		public void SetUp()
		{
			this.gradient = Gradient.GetInstance();
			this.gradient.Points.Clear();

			this.points = new GradientPoint[3];
			this.points[0] = new GradientPoint(0, 255, 0,  0);
			this.points[1] = new GradientPoint(75, 255, 255, 0);
			this.points[2] = new GradientPoint(100, 0, 255, 0);
		}

		[TearDown]
		public void TearDown()
		{
			this.gradient.Points.Clear();
		}

		[Test]
		public void TestSimpleGradient()
		{
			this.gradient.Points.Add(this.points[0].Percent, this.points[0]);
			this.gradient.Points.Add(this.points[2].Percent, this.points[2]);

			Assert.AreEqual(Color.FromArgb(0,255,0), this.gradient.GetColor(100));
			Assert.AreEqual("#00ff00", this.gradient.GetColorString(100));
			Assert.AreEqual(Color.FromArgb(255,0,0), this.gradient.GetColor(0));
			Assert.AreEqual("#ff0000", this.gradient.GetColorString(0));
			Assert.AreEqual(Color.FromArgb(127,127,0), this.gradient.GetColor(50));
			Assert.AreEqual("#7f7f00", this.gradient.GetColorString(50));
		}

		[Test]
		public void TestComplexGradient()
		{
			this.gradient.Points.Add(this.points[0].Percent, this.points[0]);
			this.gradient.Points.Add(this.points[1].Percent, this.points[1]);
			this.gradient.Points.Add(this.points[2].Percent, this.points[2]);

			Assert.AreEqual(Color.FromArgb(0,255,0), this.gradient.GetColor(100));
			Assert.AreEqual("#00ff00", this.gradient.GetColorString(100));
			Assert.AreEqual(Color.FromArgb(255,0,0), this.gradient.GetColor(0));
			Assert.AreEqual("#ff0000", this.gradient.GetColorString(0));
			Assert.AreEqual(Color.FromArgb(255,255,0), this.gradient.GetColor(75));
			Assert.AreEqual("#ffff00", this.gradient.GetColorString(75));
		}
	}
}
