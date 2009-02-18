using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using SharpCover.Instrumenting;
using SharpCover.Logging;
using SharpCover.Utilities;

namespace SharpCover.Actions
{
    /// <summary>
    /// 
    /// </summary>
	public class SharpUncoverAction : ISharpCoverAction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpUncoverAction"/> class.
        /// </summary>
		public SharpUncoverAction()
		{
		}

		private ReportSettings settings = new ReportSettings();
		private Instrumenter instrumenter = new FileCopyInstrumenter(null);

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
        /// Gets or sets the filenames.
        /// </summary>
        /// <value>The filenames.</value>
		public StringCollection Filenames
		{
			get{ return new StringCollection();}
			set{}
		}

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
		public decimal Execute()
		{
			if (File.Exists(settings.ExpectedFilename))
			{
				UninstrumentProject(settings.ExpectedFilename);
				return 0;
			}
			else
			{
				Trace.WriteLineIf(Logger.OutputType.TraceInfo, String.Format("Expected coverage file not found at {0} so won't attempt to deinstrument anything.", settings.ExpectedFilename));
				return -1;
			}
		}
		
		private void UninstrumentProject(string expected)
		{
			FileStream fs = new FileStream(expected, FileMode.Open);
			Coverage coverage = (Coverage)Serialization.FromXml(fs, typeof(Coverage));
			
			UninstrumentFiles(coverage.CoveragePoints);
			DeleteTempFiles(this.Settings);
		}

		private void DeleteTempFiles(ReportSettings settings)
		{
			if(File.Exists(settings.ActualFilename))
				File.Delete(settings.ActualFilename);

			if(File.Exists(settings.ExpectedFilename))
				File.Delete(settings.ExpectedFilename);
		}

		private void UninstrumentFiles(CoveragePoint[] points)
		{
			Hashtable done = new Hashtable();
			
			foreach (CoveragePoint point in points)
			{
				// Check if we've already deinstrumented the file associated with this point
				if (done[point.Filename] != null)
					continue;
				else
				{
					done[point.Filename] = point.Filename;
					Trace.WriteLineIf(Logger.OutputType.TraceInfo, "Deinstrumenting " + point.Filename);
					this.instrumenter.Deinstrument(point.Filename);
				}
			}
		}
	}
}