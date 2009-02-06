using System.IO;
using SharpCover.Collections;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
	public class ReportFile
	{
		public ReportFile()
		{
			this.points = new CoveragePointCollection();
		}

		private string filename;
		private CoveragePointCollection points;

		public string Filename
		{
			get{return this.filename;}
			set{this.filename = value;}
		}

		public string Name
		{
			get{return Path.GetFileName(this.filename);}
			set{}
		}

		public CoveragePointCollection Points
		{
			get{return this.points;}
			set{this.points = value;}
		}

		public int NumberOfPoints
		{
			get{return this.points.NumberOfPoints;}
			set{}
		}

		public int NumberOfHitPoints
		{
			get{return this.points.NumberOfHitPoints;}
			set{}
		}

		public decimal CoveragePercentage
		{
			get{return this.points.CoveragePercentage;}
			set{}
		}

		public string MissedLineNumbers
		{
			get{return this.points.MissedLineNumbers;}
			set{}
		}
		
		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}