using System.Collections;
using SharpCover.Parsing;

namespace SharpCover.Instrumenting
{
	public abstract class Instrumenter
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="Instrumenter"/> class.
        /// </summary>
        /// <param name="parsers">The parsers.</param>
        public Instrumenter(IParse[] parsers)
		{
			this.parsers = parsers;
		}

        protected IParse[] parsers;

		public abstract CoveragePoint[] Instrument(string filename);
		public abstract void Deinstrument(string filename);
	}
}