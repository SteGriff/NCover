using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using SharpCover.Reporting;
using SharpCover.Utilities;
using SharpCover.Logging;

namespace SharpCover.Actions
{
	public class SharpCoverReportAction : ISharpCoverAction
	{
		public SharpCoverReportAction()
		{
			settings = new ReportSettings();

			Gradient gradient = Gradient.GetInstance();
			gradient.Points.Clear();
			gradient.Add(new GradientPoint(0, 255, 0, 0));
			gradient.Add(new GradientPoint(75, 200, 200, 0));
			gradient.Add(new GradientPoint(100, 0, 200, 0));
		}

		private readonly ReportSettings settings;

		public StringCollection Filenames
		{
			get{return new StringCollection();}
			set{}
		}

		public ReportSettings Settings
		{
			get{ return settings; }
		}

		public decimal Execute()
		{
			try
			{
				CheckFileExists(settings.ActualFilename);
				CheckFileExists(settings.ExpectedFilename);
					
				Trace.WriteLineIf(Logger.OutputType.TraceInfo, String.Format("Generating Coverage Report for project {0} to \r\n{1}", 
                    settings.ReportName, settings.ReportFilename)); 
				
				var readactual = new FileStream(Settings.ActualFilename, FileMode.Open);
				Stream actual = Coverage.FixActualFile(readactual);
				var expected = new FileStream(settings.ExpectedFilename, FileMode.Open);
				
				Coverage coverage = Coverage.LoadCoverage(expected, actual);
	
				var generator = new ReportGenerator();
				Report report = generator.GenerateReport(coverage);
			
				History history = HistoryManager.LoadHistory(settings.HistoryFilename);
				HistoryManager.Update(history, report);
				HistoryManager.SaveHistory(history);
				report.History = history;
						
				HtmlReport.Generate(settings, report);

				Trace.WriteLineIf(Logger.OutputType.TraceInfo, "");
				Trace.WriteLineIf(Logger.OutputType.TraceInfo, String.Format("Overall coverage for project {1}: {0:P}", new object[] { report.CoveragePercentage, report.ReportName }));
				Trace.WriteLineIf(Logger.OutputType.TraceInfo, "");
			
				return report.CoveragePercentage;
			}
			catch(IOException ex)
			{
				Trace.WriteLine(ex.Message, "ERROR");
				return 0;
			}
		}

		private static void CheckFileExists(string filename)
		{
			if (!File.Exists(filename))
				throw new IOException("File not found: " + filename);
		}
	}
}