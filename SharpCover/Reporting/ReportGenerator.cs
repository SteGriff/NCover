using System;
using System.Collections;

namespace SharpCover.Reporting
{
	public class ReportGenerator
	{
		public ReportGenerator()
		{
		}

		public Report GenerateReport(Coverage coverage)
		{
			Report report = new Report();
			report.SharpCoverVersion = this.GetType().Assembly.GetName().Version.ToString();
			report.ReportDate = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
			report.ReportName = coverage.Settings.ReportName;
			report.SourceDir = coverage.Settings.BaseDir;
			
			PopulateNamespaces(report, coverage);

			foreach(Namespace ns in report.Namespaces)
			{
				PopulateNamespace(ns, coverage);
			}

			return report;
		}

		private void PopulateNamespaces(Report report, Coverage coverage)
		{
			foreach(CoveragePoint point in coverage.CoveragePoints)
			{
				if(!report.Namespaces.Contains(point.Namespace))
				{
					Namespace ns = new Namespace();
					ns.Name = point.Namespace;
					report.Namespaces.Add(ns);
				}
			}
		}

		private void PopulateNamespace(Namespace ns, Coverage coverage)
		{
			foreach(CoveragePoint point in coverage.CoveragePoints)
			{
				if(point.Namespace == ns.Name)
				{
					if(!ns.Files.Contains(point.Filename))
					{
						ReportFile file = new ReportFile();
						file.Filename = point.Filename;
						ns.Files.Add(file);
						PopulateFile(file, ns, coverage);
					}
				}
			}
		}

		private void PopulateFile(ReportFile file, Namespace ns, Coverage coverage)
		{
			foreach(CoveragePoint point in coverage.CoveragePoints)
			{
				if(point.Namespace == ns.Name && point.Filename == file.Filename)
				{
					file.Points.Add(point);
				}
			}
		}
	}
}
