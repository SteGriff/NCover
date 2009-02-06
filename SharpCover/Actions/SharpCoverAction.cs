using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using SharpCover.Instrumenting;
using SharpCover.Logging;

namespace SharpCover.Actions
{
	public class SharpCoverAction : ISharpCoverAction
	{
		public SharpCoverAction()
		{
			this.settings = new ReportSettings();
			this.filenames = new StringCollection();
		}

		private StringCollection filenames;
		private ReportSettings settings;
		private Instrumenter instrumenter;

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
            IList parsers = new ArrayList();
            parsers.Add(new SharpCover.Parsing.CSharp.Parser(this.settings));
            parsers.Add(new SharpCover.Parsing.VB.Parser(this.settings));
            parsers.Add(new SharpCover.Parsing.ProjectFiles.ProjectFileParser());
            this.instrumenter = new FileCopyInstrumenter(parsers);

			Trace.WriteLineIf(Logger.OutputType.TraceInfo, String.Format("Instrumenting {0} files for test coverage analysis.", this.Filenames.Count)); 
			CoveragePoint[] points = InstrumentFiles();
			
			Trace.WriteLineIf(Logger.OutputType.TraceVerbose, String.Format("Generating expected coverage document to {0}.", settings.ExpectedFilename));
			SaveCoverage(points);
			
			return points.Length;
		}

		private void SaveCoverage(CoveragePoint[] points)
		{
			Coverage coverage = new Coverage(this.Settings, points);

            string fileName = this.Settings.ExpectedFilename;

            // create directory before writing file
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            using (FileStream outputstream = new FileStream(fileName, FileMode.Create))
            {
                Coverage.SaveCoverage(outputstream, coverage);
            }
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