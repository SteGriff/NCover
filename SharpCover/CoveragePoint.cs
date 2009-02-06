using System;
using System.Xml.Serialization;

namespace SharpCover
{
	[Serializable]
	public class CoveragePoint
	{
		public CoveragePoint() 
		{
		}

		public CoveragePoint(string filename, string myNamespace, int pointNumber, int basePointNumber, bool hit)
		{
			this.filename = filename;
			this.ns = myNamespace;
			this.pointnumber = pointNumber;
			this.absolute = basePointNumber + pointnumber;
			this.hit = hit;
		}

		private string	filename;
		private string	ns;
		private int		pointnumber = -1;
		private int		absolute = -1;
		private int		lineNumber;
		private bool	hit = false;

		[XmlIgnore]
		public bool Hit 
		{
			get { return this.hit; }
			set { this.hit = value; }
		}

		[XmlAttribute()]
		public string Filename 
		{ 
			get { return this.filename; } 
			set { this.filename = value; }
		}

		[XmlAttribute()]
		public string Namespace 
		{ 
			get { return this.ns; } 
			set { this.ns = value; }
		}

		[XmlAttribute()]
		public int RelativeNumber 
		{ 
			get { return this.pointnumber; } 
			set { this.pointnumber = value; }
		}

		[XmlAttribute()]
		public int AbsoluteNumber 
		{ 
			get { return this.absolute; } 
			set { this.absolute = value; }
		}

		[XmlAttribute()]
		public int LineNumber 
		{
			get{ return this.lineNumber; }
			set{ lineNumber = value; }
		}

		public override bool Equals(object other) 
		{
			if (other == this)
				return true;

			CoveragePoint otherPoint = other as CoveragePoint;
			if (otherPoint == null)
				return false;
			
			return this.AbsoluteNumber.Equals(otherPoint.AbsoluteNumber);
		}

		public override int GetHashCode()
		{
			return this.AbsoluteNumber;
		}

		public override string ToString()
		{
			return this.filename + " covered by " + this.AbsoluteNumber + " at " + this.lineNumber; 
		}

	}
}