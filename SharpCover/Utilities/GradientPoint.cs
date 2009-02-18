using System;

namespace SharpCover.Utilities
{
    /// <summary>
    /// 
    /// </summary>
	public class GradientPoint
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientPoint"/> class.
        /// </summary>
        /// <param name="Percent">The percent.</param>
        /// <param name="Red">The red.</param>
        /// <param name="Green">The green.</param>
        /// <param name="Blue">The blue.</param>
		public GradientPoint(byte Percent, byte Red, byte Green, byte Blue)
		{
			this.percent = Percent;
			this.red = Red;
			this.green = Green;
			this.blue = Blue;
		}

		private byte	percent = 0;
		private byte	red		= 0;
		private byte	green	= 0;
		private byte	blue	= 0;

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>The percent.</value>
		public byte Percent
		{
			get{return this.percent;}
			set
			{
				if(value < 0 || value > 100)
					throw new ArgumentOutOfRangeException("The percentage must be within the range 0 - 100");
				else
					this.percent = value;
			}
		}

        /// <summary>
        /// Gets or sets the red.
        /// </summary>
        /// <value>The red.</value>
		public byte Red
		{
			get{return this.red;}
			set{this.red = value;}
		}

        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value>The blue.</value>
		public byte Blue
		{
			get{return this.blue;}
			set{this.blue = value;}
		}

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value>The green.</value>
		public byte Green
		{
			get{return this.green;}
			set{this.green = value;}
		}
	}
}