using System;
using System.IO;

namespace SharpCover
{
    /// <summary>
    /// 
    /// </summary>
	public class ReportSettings
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettings"/> class.
        /// </summary>
		public ReportSettings()
		{
		}

		private const string EXPECTED_EXTENSION = "-expected.xml";
		private const string HISTORY_EXTENSION = "-history.xml";
		private const string ACTUAL_EXTENSION = "-actual.xml";
		private const string REPORT_EXTENSION = "-report.html";

		private decimal minimum = 0;
		private string reportdir = Environment.CurrentDirectory;
		private string basedir = Environment.CurrentDirectory;
		private string reportname = "Report";

        /// <summary>
        /// Gets or sets the base dir.
        /// </summary>
        /// <value>The base dir.</value>
		public string BaseDir
		{
			get{return this.basedir;}
			set
			{
				if(value != null)
				{
					this.basedir = Path.GetFullPath(value);
				}
			}
		}

        /// <summary>
        /// Gets or sets the report dir.
        /// </summary>
        /// <value>The report dir.</value>
		public string ReportDir
		{
			get{return this.reportdir;}
			set
			{
				if(value != null)
				{
					if(!Path.IsPathRooted(value))
						this.reportdir = Path.Combine(this.BaseDir, value);
					else
						this.reportdir = value;
				}
			}
		}

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
		public string ReportName
		{
			get{return this.reportname;}
			set
			{
				if(value != null)
					this.reportname = value;
			}
		}

        /// <summary>
        /// Gets the history filename.
        /// </summary>
        /// <value>The history filename.</value>
		public string HistoryFilename
		{
			get{return GetFilename(HISTORY_EXTENSION);}
		}

        /// <summary>
        /// Gets the expected filename.
        /// </summary>
        /// <value>The expected filename.</value>
		public string ExpectedFilename
		{
			get{return GetFilename(EXPECTED_EXTENSION);}
		}

        /// <summary>
        /// Gets the actual filename.
        /// </summary>
        /// <value>The actual filename.</value>
		public string ActualFilename
		{
			get{return GetFilename(ACTUAL_EXTENSION);}
		}

        /// <summary>
        /// Gets the report filename.
        /// </summary>
        /// <value>The report filename.</value>
		public string ReportFilename
		{
			get{return GetFilename(REPORT_EXTENSION);}
		}

        /// <summary>
        /// Gets the CSS filename.
        /// </summary>
        /// <value>The CSS filename.</value>
		public string CssFilename
		{
			get{return GetFilename("sharpcover",".css");}
		}

        /// <summary>
        /// Gets or sets the minimum coverage.
        /// </summary>
        /// <value>The minimum coverage.</value>
		public decimal MinimumCoverage
		{
			get{return minimum;}
			set 
			{
				if (value > 1)
					minimum = value / 100;
				else
					minimum = value;
			}
		}

		private string GetFilename(string Extension)
		{
			return GetFilename(this.ReportName, Extension);
		}

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <param name="Filename">The filename.</param>
        /// <param name="Extension">The extension.</param>
        /// <returns></returns>
		public string GetFilename(string Filename, string Extension)
		{
			if(Path.IsPathRooted(this.ReportDir))
				return Path.Combine(this.ReportDir, Filename + Extension);
			else
				return Path.Combine(this.BaseDir, this.ReportDir + "\\" + Filename + Extension);
		}
	}
}
