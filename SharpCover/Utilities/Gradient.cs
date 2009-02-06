using System;
using System.Collections;
using System.Drawing;
using System.Text;

namespace SharpCover.Utilities
{
	public class Gradient
	{
		protected Gradient()
		{
			this.points = new SortedList();
		}

		public static Gradient GetInstance()
		{
			if(instance == null)
				instance = new Gradient();

			return instance;
		}

		private static Gradient instance;
		private SortedList points;

		public SortedList Points
		{
			get{return this.points;}
		}

		public void Add(GradientPoint point)
		{
			this.Points.Add(point.Percent, point);
		}

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