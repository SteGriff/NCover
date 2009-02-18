using System;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class Event
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
		public Event()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="Coverage">The coverage.</param>
		public Event(decimal Coverage)
		{
			this.coverage = Coverage;
		}

		private DateTime eventdate = DateTime.Now;
		private decimal coverage = 0;

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>The event date.</value>
		public DateTime EventDate
		{
			get{return this.eventdate;}
			set{this.eventdate = value;}
		}

        /// <summary>
        /// Gets or sets the coverage percentage.
        /// </summary>
        /// <value>The coverage percentage.</value>
		public decimal CoveragePercentage
		{
			get{return this.coverage;}
			set{this.coverage = value;}
		}

        /// <summary>
        /// Gets or sets the color of the coverage percentage.
        /// </summary>
        /// <value>The color of the coverage percentage.</value>
		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}
