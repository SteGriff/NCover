using System.Collections;
using System.Text;

namespace SharpCover.Collections
{
    /// <summary>
    /// 
    /// </summary>
	public class CoveragePointCollection : CollectionBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CoveragePointCollection"/> class.
        /// </summary>
		public CoveragePointCollection()
		{
		}

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="point">The point.</param>
		public void Insert(int index, CoveragePoint point)
		{
			base.InnerList.Insert(index, point);
		}

        /// <summary>
        /// Removes the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
		public void Remove(CoveragePoint point)
		{
			base.InnerList.Remove(point);
		}

        /// <summary>
        /// Adds the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
		public int Add(CoveragePoint point)
		{
			return base.InnerList.Add(point);
		}

        /// <summary>
        /// Gets the <see cref="SharpCover.CoveragePoint"/> at the specified index.
        /// </summary>
        /// <value></value>
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

        /// <summary>
        /// Gets or sets the number of hit points.
        /// </summary>
        /// <value>The number of hit points.</value>
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

        /// <summary>
        /// Gets or sets the number of points.
        /// </summary>
        /// <value>The number of points.</value>
		public int NumberOfPoints
		{
			get
			{
				return base.InnerList.Count;
			}
			set{}
		}

        /// <summary>
        /// Gets or sets the coverage percentage.
        /// </summary>
        /// <value>The coverage percentage.</value>
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

        /// <summary>
        /// Gets or sets the missed line numbers.
        /// </summary>
        /// <value>The missed line numbers.</value>
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
