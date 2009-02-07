using SharpCover.Parsing;

namespace SharpCover.Instrumenting
{
	public class MockInstrumenter : Instrumenter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="MockInstrumenter"/> class.
        /// </summary>
        /// <param name="parsers">The parsers.</param>
        public MockInstrumenter(IParse[] parsers)
            : base(parsers)
		{
		}

		private bool deinstrumentcalled = false;
		private bool instrumentcalled = false;

		public bool DeinstrumentCalled
		{
			get{return this.deinstrumentcalled;}
			set{this.deinstrumentcalled = value;}
		}

		public bool InstrumentCalled
		{
			get{return this.instrumentcalled;}
			set{this.instrumentcalled = value;}
		}

		public override void Deinstrument(string filename)
		{
			this.deinstrumentcalled = true;	
		}

		public override CoveragePoint[] Instrument(string filename)
		{
			this.instrumentcalled = true;
			CoveragePoint[] points = new CoveragePoint[1];
			points[0] = new CoveragePoint();
			return points;
		}
	}
}