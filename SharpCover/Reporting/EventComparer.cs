
namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class EventComparer : System.Collections.IComparer
	{
        /// <summary>
        /// Compares the specified o1.
        /// </summary>
        /// <param name="o1">The o1.</param>
        /// <param name="o2">The o2.</param>
        /// <returns></returns>
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