
namespace SharpCover
{
	public class CoveragePointComparer : System.Collections.IComparer
	{
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