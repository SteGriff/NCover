
namespace SharpCover
{
    /// <summary>
    /// 
    /// </summary>
	public class CoveragePointComparer : System.Collections.IComparer
	{
        /// <summary>
        /// Compares the specified o1.
        /// </summary>
        /// <param name="o1">The o1.</param>
        /// <param name="o2">The o2.</param>
        /// <returns></returns>
		public int Compare(object o1, object o2)
		{
			CoveragePoint c1 = (CoveragePoint) o1;
			CoveragePoint c2 = (CoveragePoint) o2;

			if (c1.LineNumber == c2.LineNumber)
				return 0;

			if (c1.LineNumber > c2.LineNumber)
				return 1;
			else
				return -1;
		}
	}
}