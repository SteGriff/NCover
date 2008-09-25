using System;
using System.Drawing;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	public class Event
	{
		public Event()
		{
		}

		public Event(decimal Coverage)
		{
			this.coverage = Coverage;
		}

		private DateTime eventdate = DateTime.Now;
		private decimal coverage = 0;

		public DateTime EventDate
		{
			get{return this.eventdate;}
			set{this.eventdate = value;}
		}

		public decimal CoveragePercentage
		{
			get{return this.coverage;}
			set{this.coverage = value;}
		}

		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}
