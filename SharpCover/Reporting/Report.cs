using SharpCover.Collections;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class Report
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// <value>The history.</value>
		public History History
		{
			get{return this.history;}
			set{this.history = value;}
		}

        /// <summary>
        /// Gets or sets the namespaces.
        /// </summary>
        /// <value>The namespaces.</value>
		public NamespaceCollection Namespaces
		{
			get{return this.namespaces;}
			set{this.namespaces = value;}
		}

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
		public string ReportName
		{
			get{return this.reportname;}
			set{this.reportname = value;}
		}

        /// <summary>
        /// Gets or sets the source dir.
        /// </summary>
        /// <value>The source dir.</value>
		public string SourceDir
		{
			get{return this.sourcedir;}
			set{this.sourcedir = value;}
		}

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        /// <value>The report date.</value>
		public string ReportDate
		{
			get{return this.reportdate;}
			set{this.reportdate = value;}
		}

        /// <summary>
        /// Gets or sets the sharp cover version.
        /// </summary>
        /// <value>The sharp cover version.</value>
		public string SharpCoverVersion
		{
			get{return this.sharpcoverversion;}
			set{this.sharpcoverversion = value;}
		}

        /// <summary>
        /// Gets or sets the number of points.
        /// </summary>
        /// <value>The number of points.</value>
		public int NumberOfPoints
		{
			get{return this.namespaces.NumberOfPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the number of hit points.
        /// </summary>
        /// <value>The number of hit points.</value>
		public int NumberOfHitPoints
		{
			get{return this.namespaces.NumberOfHitPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the coverage percentage.
        /// </summary>
        /// <value>The coverage percentage.</value>
		public decimal CoveragePercentage
		{
			get{return this.namespaces.CoveragePercentage;}
			set{}
		}

        /// <summary>
        /// Gets or sets the color of the coverage percentage.
        /// </summary>
        /// <value>The color of the coverage percentage.</value>
		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}
