using System.Collections;

using SharpCover.Reporting;

namespace SharpCover.Collections
{
    /// <summary>
    /// 
    /// </summary>
	public class NamespaceCollection : CollectionBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceCollection"/> class.
        /// </summary>
		public NamespaceCollection()
		{
		}

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="reportnamespace">The reportnamespace.</param>
		public void Insert(int index, Namespace reportnamespace)
		{
			base.InnerList.Insert(index, reportnamespace);
		}

        /// <summary>
        /// Removes the specified reportnamespace.
        /// </summary>
        /// <param name="reportnamespace">The reportnamespace.</param>
		public void Remove(Namespace reportnamespace)
		{
			base.InnerList.Remove(reportnamespace);
		}

        /// <summary>
        /// Adds the specified reportnamespace.
        /// </summary>
        /// <param name="reportnamespace">The reportnamespace.</param>
        /// <returns></returns>
		public int Add(Namespace reportnamespace)
		{
			return base.InnerList.Add(reportnamespace);
		}

        /// <summary>
        /// Determines whether [contains] [the specified reportnamespace].
        /// </summary>
        /// <param name="reportnamespace">The reportnamespace.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified reportnamespace]; otherwise, <c>false</c>.
        /// </returns>
		public bool Contains(Namespace reportnamespace)
		{
			foreach(Namespace ns in base.InnerList)
			{
				if(ns == reportnamespace)
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
			foreach(Namespace ns in base.InnerList)
			{
				if(ns.Name == name)
					return true;
			}

			return false;
		}

        /// <summary>
        /// Gets the <see cref="SharpCover.Reporting.Namespace"/> at the specified index.
        /// </summary>
        /// <value></value>
		public Namespace this[int index]
		{
			get
			{
				if(index < 0 || index >= this.Count)
					return null;
				else
					return (Namespace)base.InnerList[index];
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
				foreach(Namespace reportnamespace in base.List)
				{
					retval += reportnamespace.Files.NumberOfHitPoints;
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
				foreach(Namespace reportnamespace in base.List)
				{
					retval += reportnamespace.Files.NumberOfPoints;
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