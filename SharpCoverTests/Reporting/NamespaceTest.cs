using System;
using NUnit.Framework;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class NamespaceTest
	{
		public NamespaceTest()
		{
		}

		private Namespace ns;

		[SetUp]
		public void SetUp()
		{
			this.ns = new Namespace();
		}

		[Test]
		public void TestDefaults()
		{
			Assert.IsNotNull(this.ns.Files);
			Assert.AreEqual(0, this.ns.Files.Count);

			Assert.AreEqual(0, this.ns.NumberOfPoints);
			Assert.AreEqual(0, this.ns.NumberOfHitPoints);
			Assert.AreEqual(0, this.ns.CoveragePercentage);

			try
			{
				string s = this.ns.CoveragePercentageColor;
				Assert.Fail();
			}
			catch(InvalidOperationException)
			{
			}

			Assert.IsNull(this.ns.Name);
		}
	}
}
