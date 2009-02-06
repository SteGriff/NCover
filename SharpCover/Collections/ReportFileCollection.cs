using System.Collections;

using SharpCover.Reporting;

namespace SharpCover.Collections
{
	public class ReportFileCollection : CollectionBase
	{
		public ReportFileCollection()
		{
		}

		public void Insert(int index, ReportFile file)
		{
			base.InnerList.Insert(index, file);
		}

		public void Remove(ReportFile file)
		{
			base.InnerList.Remove(file);
		}

		public int Add(ReportFile file)
		{
			return base.InnerList.Add(file);
		}

		public bool Contains(ReportFile file)
		{
			foreach(ReportFile rf in base.InnerList)
			{
				if(rf == file)
					return true;
			}

			return false;
		}

		public bool Contains(string name)
		{
			foreach(ReportFile file in base.InnerList)
			{
				if(file.Filename == name)
					return true;
			}

			return false;
		}

		public ReportFile this[int index]
		{
			get
			{
				if(index < 0 || index >= this.Count)
					return null;
				else
					return (ReportFile)base.InnerList[index];
			}
		}

		public int NumberOfHitPoints
		{
			get
			{
				int retval = 0;
				foreach(ReportFile file in base.List)
				{
					retval += file.Points.NumberOfHitPoints;
				}

				return retval;
			}
			set{}
		}

		public int NumberOfPoints
		{
			get
			{
				int retval = 0;
				foreach(ReportFile file in base.List)
				{
					retval += file.Points.NumberOfPoints;
				}

				return retval;
			}
			set{}
		}

		public decimal CoveragePercentage
		{
			get
			{
				decimal retval = 0;
				if(NumberOfPoints > 0)
					retval = (decimal)this.NumberOfHitPoints / (decimal)this.NumberOfPoints;
 
				return retval;
			}
			set{}
		}
	}
}
