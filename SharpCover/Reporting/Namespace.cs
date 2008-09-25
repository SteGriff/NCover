using System;
using SharpCover.Utilities;
using SharpCover.Collections;

namespace SharpCover.Reporting
{
	public class Namespace
	{
		public Namespace()
		{
			this.files = new ReportFileCollection();
		}

		private string name;
		private ReportFileCollection files;

		public string Name
		{
			get{return this.name;}
			set{this.name = value;}
		}

		public ReportFileCollection Files
		{
			get{return this.files;}
			set{this.files = value;}
		}

		public int NumberOfPoints
		{
			get{return this.files.NumberOfPoints;}
			set{}
		}

		public int NumberOfHitPoints
		{
			get{return this.files.NumberOfHitPoints;}
			set{}
		}

		public decimal CoveragePercentage
		{
			get{return this.files.CoveragePercentage;}
			set{}
		}
		
		public string CoveragePercentageColor
		{
			get{return Gradient.GetInstance().GetColorString((byte)(this.CoveragePercentage * 100));}
			set{}
		}
	}
}
