using System;
using NUnit.Framework;

namespace SharpCover.Reporting
{
	[TestFixture]
	public class EventTest
	{
		public EventTest()
		{
		}

		private decimal dec = 1.12323M;
		private DateTime date = DateTime.Now;

		[Test]
		public void TestProperties()
		{
			Event evnt = new Event();
			
			evnt.CoveragePercentage = this.dec;
			evnt.EventDate = this.date;

			Assert.AreEqual(this.dec, evnt.CoveragePercentage, "CoveragePercentage get/set not working");
			Assert.AreEqual(this.date, evnt.EventDate, "EventDate get/set not working");
		}

		[Test]
		public void TestConstructor()
		{
			Event evnt = new Event(this.dec);
			Assert.AreEqual(this.dec, evnt.CoveragePercentage, "CoveragePercentage should be set by constructor");
	
			try
			{
				string s = evnt.CoveragePercentageColor;
				Assert.Fail();
			}
			catch(InvalidOperationException)
			{
			}
			catch(ArgumentOutOfRangeException)
			{
			}
		}
	}
}
