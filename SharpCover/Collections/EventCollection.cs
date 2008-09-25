using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using SharpCover.Reporting;

namespace SharpCover.Collections
{
	public class EventCollection : CollectionBase
	{
		public EventCollection()
		{
		}

		public void Insert(int index, Event evnt)
		{
			base.InnerList.Insert(index, evnt);
		}

		public void Remove(Event evnt)
		{
			base.InnerList.Remove(evnt);
		}

		public int Add(Event evnt)
		{
			return base.InnerList.Add(evnt);
		}

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

		public void Sort(IComparer comparer)
		{
			base.InnerList.Sort(comparer);
		}
	}
}