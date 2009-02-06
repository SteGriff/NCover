using SharpCover.Collections;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	public class Report
	{
		public Report()
		{
			this.namespaces = new NamespaceCollection();
		}

		private History history;
		private string reportname;
		private string sourcedir;
		private string reportdate;
		private string sharpcoverversion;
		private NamespaceCollection namespaces;

		public History History
		{
			get{return this.history;}
			set{this.history = value;}
		}

		public NamespaceCollection Namespaces
		{
			get{return this.namespaces;}
			set{this.namespaces = value;}
		}

		public string ReportName
		{
			get{return this.reportname;}
			set{this.reportname = value;}
		}

		public string SourceDir
		{
			get{return this.sourcedir;}
			set{this.sourcedir = value;}
		}

		public string ReportDate
		{
			get{return this.reportdate;}
			set{this.reportdate = value;}
		}

		public string SharpCoverVersion
		{
			get{return this.sharpcoverversion;}
			set{this.sharpcoverversion = value;}
		}

		public int NumberOfPoints
		{
			get{return this.namespaces.NumberOfPoints;}
			set{}
		}

		public int NumberOfHitPoints
		{
			get{return this.namespaces.NumberOfHitPoints;}
			set{}
		}

		public decimal CoveragePercentage
		{
			get{return this.namespaces.CoveragePercentage;}
			set{}
		}

		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}
