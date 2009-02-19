using System;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class ReportGenerator
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerator"/> class.
        /// </summary>
		public ReportGenerator()
		{
		}

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="coverage">The coverage.</param>
        /// <returns></returns>
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
				if(!string.IsNullOrEmpty(point.Namespace) && !report.Namespaces.Contains(point.Namespace))
				{
					report.Namespaces.Add(new Namespace() { Name = point.Namespace });
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
                        ReportFile file = new ReportFile() { Filename = point.Filename };
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
