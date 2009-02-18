using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using SharpCover.Instrumenting;
using SharpCover.Logging;
using SharpCover.Parsing;

namespace SharpCover.Actions
{
    /// <summary>
    /// 
    /// </summary>
	public class SharpCoverAction : ISharpCoverAction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCoverAction"/> class.
        /// </summary>
		public SharpCoverAction()
		{
			this.settings = new ReportSettings();
			this.filenames = new StringCollection();
		}

		private StringCollection filenames;
		private ReportSettings settings;
		private Instrumenter instrumenter;
        private readonly CoveragePoint[] emptyPoints = new CoveragePoint[0];

        /// <summary>
        /// Gets or sets the filenames.
        /// </summary>
        /// <value>The filenames.</value>
		public StringCollection Filenames
		{
			get{return this.filenames;}
			set{this.filenames = value;}
		}

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
		public ReportSettings Settings
		{
			get{return this.settings;}
			set{this.settings = value;}
		}

        /// <summary>
        /// Gets or sets the instrumenter.
        /// </summary>
        /// <value>The instrumenter.</value>
		public Instrumenter Instrumenter
		{
			get{return this.instrumenter;}
			set{this.instrumenter = value;}
		}

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
		public decimal Execute()
		{
            List<IParse> parsers = new List<IParse>(3);
            parsers.Add(new SharpCover.Parsing.CSharp.Parser(this.settings));
            parsers.Add(new SharpCover.Parsing.VB.Parser(this.settings));
            parsers.Add(new SharpCover.Parsing.ProjectFiles.ProjectFileParser());
            this.instrumenter = new FileCopyInstrumenter(parsers.ToArray());

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
            List<CoveragePoint> coveragePoints = new List<CoveragePoint>();
			
			foreach(string filename in this.Filenames)
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "instrumenting file: " + filename);
				
				coveragePoints.AddRange(this.instrumenter.Instrument(filename) ?? this.emptyPoints);
			}
		
			return coveragePoints.ToArray();
		}
	}
}