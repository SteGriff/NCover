using System.IO;
using SharpCover.Collections;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public class ReportFile
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportFile"/> class.
        /// </summary>
		public ReportFile()
		{
			this.points = new CoveragePointCollection();
		}

		private string filename;
		private CoveragePointCollection points;

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
		public string Filename
		{
			get{return this.filename;}
			set{this.filename = value;}
		}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get{return Path.GetFileName(this.filename);}
			set{}
		}

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
		public CoveragePointCollection Points
		{
			get{return this.points;}
			set{this.points = value;}
		}

        /// <summary>
        /// Gets or sets the number of points.
        /// </summary>
        /// <value>The number of points.</value>
		public int NumberOfPoints
		{
			get{return this.points.NumberOfPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the number of hit points.
        /// </summary>
        /// <value>The number of hit points.</value>
		public int NumberOfHitPoints
		{
			get{return this.points.NumberOfHitPoints;}
			set{}
		}

        /// <summary>
        /// Gets or sets the coverage percentage.
        /// </summary>
        /// <value>The coverage percentage.</value>
		public decimal CoveragePercentage
		{
			get{return this.points.CoveragePercentage;}
			set{}
		}

        /// <summary>
        /// Gets or sets the missed line numbers.
        /// </summary>
        /// <value>The missed line numbers.</value>
		public string MissedLineNumbers
		{
			get{return this.points.MissedLineNumbers;}
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