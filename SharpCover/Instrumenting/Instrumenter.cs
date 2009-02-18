using System.Collections;
using SharpCover.Parsing;

namespace SharpCover.Instrumenting
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        protected IParse[] parsers;

        /// <summary>
        /// Instruments the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
		public abstract CoveragePoint[] Instrument(string filename);
        /// <summary>
        /// Deinstruments the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
		public abstract void Deinstrument(string filename);
	}
}