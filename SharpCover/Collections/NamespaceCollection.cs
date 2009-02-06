using System.Collections;

using SharpCover.Reporting;

namespace SharpCover.Collections
{
	public class NamespaceCollection : CollectionBase
	{
		public NamespaceCollection()
		{
		}

		public void Insert(int index, Namespace reportnamespace)
		{
			base.InnerList.Insert(index, reportnamespace);
		}

		public void Remove(Namespace reportnamespace)
		{
			base.InnerList.Remove(reportnamespace);
		}

		public int Add(Namespace reportnamespace)
		{
			return base.InnerList.Add(reportnamespace);
		}

		public bool Contains(Namespace reportnamespace)
		{
			foreach(Namespace ns in base.InnerList)
			{
				if(ns == reportnamespace)
					return true;
			}

			return false;
		}

		public bool Contains(string name)
		{
			foreach(Namespace ns in base.InnerList)
			{
				if(ns.Name == name)
					return true;
			}

			return false;
		}

		public Namespace this[int index]
		{
			get
			{
				if(index < 0 || index >= this.Count)
					return null;
				else
					return (Namespace)base.InnerList[index];
			}
		}

		public int NumberOfHitPoints
		{
			get
			{
				int retval = 0;
				foreach(Namespace reportnamespace in base.List)
				{
					retval += reportnamespace.Files.NumberOfHitPoints;
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
				foreach(Namespace reportnamespace in base.List)
				{
					retval += reportnamespace.Files.NumberOfPoints;
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