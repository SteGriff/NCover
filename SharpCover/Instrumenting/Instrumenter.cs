using System.Collections;

namespace SharpCover.Instrumenting
{
	public abstract class Instrumenter
	{
		/// <summary>
		/// IList of parsers
		/// </summary>
		public Instrumenter(IList parsers)
		{
			this.parsers = parsers;
		}
	
		protected IList parsers;

		public abstract CoveragePoint[] Instrument(string filename);
		public abstract void Deinstrument(string filename);
	}
}