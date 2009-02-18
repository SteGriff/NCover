using System;
using System.Collections;
using System.Drawing;
using System.Text;

namespace SharpCover.Utilities
{
    /// <summary>
    /// 
    /// </summary>
	public class Gradient
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Gradient"/> class.
        /// </summary>
		protected Gradient()
		{
			this.points = new SortedList();
		}

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
		public static Gradient GetInstance()
		{
			if(instance == null)
				instance = new Gradient();

			return instance;
		}

		private static Gradient instance;
		private SortedList points;

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
		public SortedList Points
		{
			get{return this.points;}
		}

        /// <summary>
        /// Adds the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
		public void Add(GradientPoint point)
		{
			this.Points.Add(point.Percent, point);
		}

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="Percentage">The percentage.</param>
        /// <returns></returns>
		public Color GetColor(byte Percentage)
		{
			if(this.points[(byte)0] == null || this.points[(byte)100] == null)
				throw new InvalidOperationException("There must be at least two gradient points in the list:- 0% and 100%");

			if(Percentage < 0 || Percentage > 100)
				throw new ArgumentOutOfRangeException("The percentage must be within the range 0 - 100");
			
			GradientPoint[] gps = GetNearestPoints(Percentage);
			byte red = CalculateValue(gps[0].Red, gps[1].Red, gps[0].Percent, gps[1].Percent, Percentage);
			byte green = CalculateValue(gps[0].Green, gps[1].Green, gps[0].Percent, gps[1].Percent, Percentage);
			byte blue = CalculateValue(gps[0].Blue, gps[1].Blue, gps[0].Percent, gps[1].Percent, Percentage);

			return Color.FromArgb(red, green, blue);
		}

        /// <summary>
        /// Gets the color string.
        /// </summary>
        /// <param name="Percentage">The percentage.</param>
        /// <returns></returns>
		public string GetColorString(byte Percentage)
		{
			Color color = this.GetColor(Percentage);
			StringBuilder sb = new StringBuilder(7,7);
			sb.Append("#");
			sb.Append(GetString(color.R));
			sb.Append(GetString(color.G));
			sb.Append(GetString(color.B));

			return sb.ToString().ToLower();
		}

		private string GetString(byte colorvalue)
		{
			string retval = colorvalue.ToString("X");
			if(retval.Length == 1)
				retval = retval.Insert(0, "0");

			return retval;
		}

		private byte CalculateValue(byte v1, byte v2, byte p1, byte p2, byte percentage)
		{
			decimal range = p2 - p1;
			decimal adjustedpercent = percentage - p1;

			return (byte) (v1 - ((v1 - v2) / range * adjustedpercent));
		}

		private GradientPoint[] GetNearestPoints(byte Percentage)
		{
			GradientPoint[] retval = new GradientPoint[2];
			
			int length = this.points.Count;
			for(int i=0; i < length; i++)
			{
				GradientPoint point = (GradientPoint)this.points.GetByIndex(i);
				if(point.Percent <= Percentage)
				{
					retval[0] = point;

					if(i + 1 != length)
					{
						GradientPoint nextpoint = (GradientPoint)this.points.GetByIndex(i+1);
						if(nextpoint.Percent >= Percentage)
						{
							retval[1] = nextpoint;
							break;
						}
					}
				}
			}

			return retval;
		}	
	}
}