using System;

namespace SharpCover.Reporting
{
	public class EventComparer : System.Collections.IComparer
	{
		public int Compare(object o1, object o2)
		{
			Event e1 = (Event) o1;
			Event e2 = (Event) o2;

			if (e1.EventDate == e2.EventDate)
				return 0;

			return e1.EventDate < e2.EventDate? 1 : -1;
		}
	}
}