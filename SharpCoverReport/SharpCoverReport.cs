using System;
using System.Collections.Specialized;
using SharpCover.Actions;
using SharpCover.Logging;
using SharpCover.Utilities;

namespace SharpCover.CommandLine
{
	class SharpCoverReport
	{
		[STAThread]
		static void Main(string[] args)
		{
			Logger.AddConsoleListener("SharpCoverReport");

			NameValueCollection parameters = ArgumentParsing.ParseCommandLine(args);

			var sharpcoverReportAction = new SharpCoverReportAction();
			if (parameters[Constants.MINIMUM] != null)
			{
				sharpcoverReportAction.Settings.MinimumCoverage = Decimal.Parse(parameters[Constants.MINIMUM]);
			}
			sharpcoverReportAction.Settings.BaseDir = parameters[Constants.BASE_DIR];
			sharpcoverReportAction.Settings.ReportName = parameters[Constants.REPORT_NAME];
			sharpcoverReportAction.Settings.ReportDir = parameters[Constants.REPORT_DIR];
			
			decimal testCoverage = sharpcoverReportAction.Execute();

			if (testCoverage < sharpcoverReportAction.Settings.MinimumCoverage)
			{
				Console.Error.WriteLine(String.Format("Test coverage has fallen below minimum standard of {0:P}, coverage now at {1:P}", sharpcoverReportAction.Settings.MinimumCoverage, testCoverage));
				Environment.Exit(-2);
			}
			else
			{
				if (testCoverage < (sharpcoverReportAction.Settings.MinimumCoverage + (decimal)0.1))
				{
					Console.Error.WriteLine(String.Format("WARNING: Test coverage within 10% of {0:P} minimum (coverage now {1:P})", sharpcoverReportAction.Settings.MinimumCoverage, testCoverage));
					Environment.Exit(-1);
				}
			}
			Environment.Exit(0);			
		}
	}
}