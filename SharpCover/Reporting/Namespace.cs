using SharpCover.Collections;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class Namespace
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
		public Namespace()
		{
			this.files = new ReportFileCollection();
		}

		private string name;
		private ReportFileCollection files;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get{return this.name;}
			set{this.name = value;}
		}

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>The files.</value>
		public ReportFileCollection Files
		{
			get{return this.files;}
			set{this.files = value;}
		}

        /// <summary>
        /// Gets or sets the number of points.
        /// </summary>
        /// <value>The number of points.</value>
		public int NumberOfPoints
		{
			get{return this.files.NumberOfPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the number of hit points.
        /// </summary>
        /// <value>The number of hit points.</value>
		public int NumberOfHitPoints
		{
			get{return this.files.NumberOfHitPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the coverage percentage.
        /// </summary>
        /// <value>The coverage percentage.</value>
		public decimal CoveragePercentage
		{
			get{return this.files.CoveragePercentage;}
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
