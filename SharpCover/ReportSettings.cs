using System;
using System.IO;

namespace SharpCover
{
	public class ReportSettings
	{
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

		public string ReportName
		{
			get{return this.reportname;}
			set
			{
				if(value != null)
					this.reportname = value;
			}
		}

		public string HistoryFilename
		{
			get{return GetFilename(HISTORY_EXTENSION);}
		}

		public string ExpectedFilename
		{
			get{return GetFilename(EXPECTED_EXTENSION);}
		}

		public string ActualFilename
		{
			get{return GetFilename(ACTUAL_EXTENSION);}
		}

		public string ReportFilename
		{
			get{return GetFilename(REPORT_EXTENSION);}
		}

		public string CssFilename
		{
			get{return GetFilename("sharpcover",".css");}
		}

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

		public string GetFilename(string Filename, string Extension)
		{
			if(Path.IsPathRooted(this.ReportDir))
				return Path.Combine(this.ReportDir, Filename + Extension);
			else
				return Path.Combine(this.BaseDir, this.ReportDir + "\\" + Filename + Extension);
		}
	}
}
