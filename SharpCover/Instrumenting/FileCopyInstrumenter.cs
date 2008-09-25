using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using SharpCover.Logging;
using SharpCover.Parsing;

namespace SharpCover.Instrumenting
{
	public class FileCopyInstrumenter : Instrumenter
	{
		public FileCopyInstrumenter(IList parsers) : base(parsers)
		{
		}

		/// <summary>
		/// Instruments with first acceptable parser.
		/// </summary>
		public override CoveragePoint[] Instrument(string filename)
		{				
			CoveragePoint[] points = null;
			
			foreach (IParse parser in parsers)
			{
				if (parser.Accept(filename))
				{
					string instrumentedSrc = parser.Parse(filename);
					points = parser.CoveragePoints;

					Trace.WriteLineIf(Logger.OutputType.TraceVerbose, String.Format("File: {0}, Points: {1}", new object[] { filename, points.Length }));

					if (points.Length == 0)
						return points;			
			
					SaveUninstrumentedFile(filename);
					WriteInstrumentedSource(instrumentedSrc, filename);		
				}
			}
			
			return points;
		}
        
		public override void Deinstrument(string filename)
		{
			string deinstFilename = filename + ".instrumented";

			if (File.Exists(deinstFilename))
			{
				File.Delete(filename);
				File.Move(deinstFilename, filename);
			}
		}

		private void SaveUninstrumentedFile(string filename)
		{
			Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "Saving original " + filename);

			if (File.Exists(filename) && !File.Exists(filename + ".instrumented"))
			{
				File.Copy(filename, filename + ".instrumented", true);
			}
		}

		private void WriteInstrumentedSource(string source, string filename)
		{
			using (Stream s = new FileStream(filename, FileMode.Create))
			{
				using (TextWriter writer = new StreamWriter(s))
				{
					writer.WriteLine(source);
					writer.Flush();
					writer.Close();					
				}
				s.Close();
			}
		}
	}
}