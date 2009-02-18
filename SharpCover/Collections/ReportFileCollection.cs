using System.Collections;

using SharpCover.Reporting;

namespace SharpCover.Collections
{
    /// <summary>
    /// 
    /// </summary>
	public class ReportFileCollection : CollectionBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportFileCollection"/> class.
        /// </summary>
		public ReportFileCollection()
		{
		}

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="file">The file.</param>
		public void Insert(int index, ReportFile file)
		{
			base.InnerList.Insert(index, file);
		}

        /// <summary>
        /// Removes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
		public void Remove(ReportFile file)
		{
			base.InnerList.Remove(file);
		}

        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
		public int Add(ReportFile file)
		{
			return base.InnerList.Add(file);
		}

        /// <summary>
        /// Determines whether [contains] [the specified file].
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified file]; otherwise, <c>false</c>.
        /// </returns>
		public bool Contains(ReportFile file)
		{
			foreach(ReportFile rf in base.InnerList)
			{
				if(rf == file)
					return true;
			}

			return false;
		}

        /// <summary>
        /// Determines whether [contains] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
		public bool Contains(string name)
		{
			foreach(ReportFile file in base.InnerList)
			{
				if(file.Filename == name)
					return true;
			}

			return false;
		}

        /// <summary>
        /// Gets the <see cref="SharpCover.Reporting.ReportFile"/> at the specified index.
        /// </summary>
        /// <value></value>
		public ReportFile this[int index]
		{
			get
			{
				if(index < 0 || index >= this.Count)
					return null;
				else
					return (ReportFile)base.InnerList[index];
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
				foreach(ReportFile file in base.List)
				{
					retval += file.Points.NumberOfHitPoints;
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
				int retval = 0;
				foreach(ReportFile file in base.List)
				{
					retval += file.Points.NumberOfPoints;
				}

				return retval;
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
				if(NumberOfPoints > 0)
					retval = (decimal)this.NumberOfHitPoints / (decimal)this.NumberOfPoints;
 
				return retval;
			}
			set{}
		}
	}
}
