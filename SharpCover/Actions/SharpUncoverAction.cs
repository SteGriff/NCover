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
	public class SharpUncoverAction : ISharpCoverAction
	{
		public SharpUncoverAction()
		{
		}

		private ReportSettings settings = new ReportSettings();
		private Instrumenter instrumenter = new FileCopyInstrumenter(null);

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

		public StringCollection Filenames
		{
			get{ return new StringCollection();}
			set{}
		}

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