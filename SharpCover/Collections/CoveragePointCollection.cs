using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

namespace SharpCover.Collections
{
	public class CoveragePointCollection : CollectionBase
	{
		public CoveragePointCollection()
		{
		}

		public void Insert(int index, CoveragePoint point)
		{
			base.InnerList.Insert(index, point);
		}

		public void Remove(CoveragePoint point)
		{
			base.InnerList.Remove(point);
		}

		public int Add(CoveragePoint point)
		{
			return base.InnerList.Add(point);
		}

		public CoveragePoint this[int index]
		{
			get
			{
				if(index < 0 || index >= this.Count)
					return null;
				else
					return (CoveragePoint)base.InnerList[index];
			}
		}

		public int NumberOfHitPoints
		{
			get
			{
				int retval = 0;
				foreach(CoveragePoint point in base.List)
				{
					if(point.Hit)
						retval ++;
				}

				return retval;
			}
			set{}
		}

		public int NumberOfPoints
		{
			get
			{
				return base.InnerList.Count;
			}
			set{}
		}

		public decimal CoveragePercentage
		{
			get
			{
				decimal retval = 0;
				if(base.InnerList.Count > 0)
                    retval = (decimal)this.NumberOfHitPoints / (decimal)base.InnerList.Count;
 
				return retval;
			}
			set{}
		}

		public string MissedLineNumbers
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				
				base.InnerList.Sort(new CoveragePointComparer());
				foreach(CoveragePoint point in base.InnerList)
				{
					if(!point.Hit)
					{
						if(sb.Length > 0)
							sb.Append(", ");

						sb.Append(point.LineNumber.ToString());
					}
				}

				return sb.ToString();
			}
			set{}
		}
	}
}
