using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using SharpCover.Logging;
using SharpCover.Instrumenting;

namespace SharpCover.Actions
{
	public class SharpCoverAction : ISharpCoverAction
	{
		public SharpCoverAction()
		{
			this.settings = new ReportSettings();
			this.filenames = new StringCollection();
			IList parsers = new ArrayList();
			parsers.Add(new SharpCover.Parsing.CSharp.Parser(this.settings));
			parsers.Add(new SharpCover.Parsing.VB.Parser(this.settings));
			this.instrumenter = new FileCopyInstrumenter(parsers);
		}

		private StringCollection	filenames;
		private ReportSettings		settings;
		private Instrumenter		instrumenter;

		public StringCollection Filenames
		{
			get{return this.filenames;}
			set{this.filenames = value;}
		}

		public ReportSettings Settings
		{
			get{return this.settings;}
			set{this.settings = value;}
		}

		public Instrumenter Instrumenter
		{
			get{return this.instrumenter;}
			set{this.instrumenter = value;}
		}

		public decimal Execute()
		{
			Trace.WriteLineIf(Logger.OutputType.TraceInfo, String.Format("Instrumenting {0} files for test coverage analysis.", this.Filenames.Count)); 
			CoveragePoint[] points = InstrumentFiles();
			
			Trace.WriteLineIf(Logger.OutputType.TraceVerbose, String.Format("Generating expected coverage document to {0}.", settings.ExpectedFilename));
			SaveCoverage(points);
			
			return points.Length;
		}

		private void SaveCoverage(CoveragePoint[] points)
		{
			Coverage coverage = new Coverage(this.Settings, points);
			FileStream outputstream = new FileStream(this.Settings.ExpectedFilename, FileMode.Create);
			
			Coverage.SaveCoverage(outputstream, coverage);
		}

		private CoveragePoint[] InstrumentFiles()
		{
			ArrayList coveragePoints = new ArrayList();
			
			foreach(string filename in this.Filenames)
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "instrumenting file: " + filename);
				
				coveragePoints.AddRange(this.instrumenter.Instrument(filename));
			}
		
			return (CoveragePoint[])coveragePoints.ToArray(typeof(CoveragePoint));
		}
	}
}