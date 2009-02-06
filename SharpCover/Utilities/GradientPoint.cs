using System;

namespace SharpCover.Utilities
{
	public class GradientPoint
	{
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

		public byte Red
		{
			get{return this.red;}
			set{this.red = value;}
		}

		public byte Blue
		{
			get{return this.blue;}
			set{this.blue = value;}
		}

		public byte Green
		{
			get{return this.green;}
			set{this.green = value;}
		}
	}
}