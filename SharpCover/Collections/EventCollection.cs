using System.Collections;
using SharpCover.Reporting;

namespace SharpCover.Collections
{
    /// <summary>
    /// 
    /// </summary>
	public class EventCollection : CollectionBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCollection"/> class.
        /// </summary>
		public EventCollection()
		{
		}

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="evnt">The evnt.</param>
		public void Insert(int index, Event evnt)
		{
			base.InnerList.Insert(index, evnt);
		}

        /// <summary>
        /// Removes the specified evnt.
        /// </summary>
        /// <param name="evnt">The evnt.</param>
		public void Remove(Event evnt)
		{
			base.InnerList.Remove(evnt);
		}

        /// <summary>
        /// Adds the specified evnt.
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
		public int Add(Event evnt)
		{
			return base.InnerList.Add(evnt);
		}

        /// <summary>
        /// Gets the <see cref="SharpCover.Reporting.Event"/> at the specified index.
        /// </summary>
        /// <value></value>
		public Event this[int index]
		{
			get
			{
				if(index >= this.Count || index < 0)
					return null;
				else
					return (Event)base.InnerList[index];
			}
		}

        /// <summary>
        /// Sorts the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
		public void Sort(IComparer comparer)
		{
			base.InnerList.Sort(comparer);
		}
	}
}