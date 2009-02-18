using System;
using System.Xml.Serialization;

namespace SharpCover
{
    /// <summary>
    /// 
    /// </summary>
	[Serializable]
	public class CoveragePoint
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CoveragePoint"/> class.
        /// </summary>
		public CoveragePoint() 
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="CoveragePoint"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="myNamespace">My namespace.</param>
        /// <param name="pointNumber">The point number.</param>
        /// <param name="basePointNumber">The base point number.</param>
        /// <param name="hit">if set to <c>true</c> [hit].</param>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CoveragePoint"/> is hit.
        /// </summary>
        /// <value><c>true</c> if hit; otherwise, <c>false</c>.</value>
		[XmlIgnore]
		public bool Hit 
		{
			get { return this.hit; }
			set { this.hit = value; }
		}

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
		[XmlAttribute()]
		public string Filename 
		{ 
			get { return this.filename; } 
			set { this.filename = value; }
		}

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
		[XmlAttribute()]
		public string Namespace 
		{ 
			get { return this.ns; } 
			set { this.ns = value; }
		}

        /// <summary>
        /// Gets or sets the relative number.
        /// </summary>
        /// <value>The relative number.</value>
		[XmlAttribute()]
		public int RelativeNumber 
		{ 
			get { return this.pointnumber; } 
			set { this.pointnumber = value; }
		}

        /// <summary>
        /// Gets or sets the absolute number.
        /// </summary>
        /// <value>The absolute number.</value>
		[XmlAttribute()]
		public int AbsoluteNumber 
		{ 
			get { return this.absolute; } 
			set { this.absolute = value; }
		}

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>
		[XmlAttribute()]
		public int LineNumber 
		{
			get{ return this.lineNumber; }
			set{ lineNumber = value; }
		}

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
		public override bool Equals(object other) 
		{
			if (other == this)
				return true;

			CoveragePoint otherPoint = other as CoveragePoint;
			if (otherPoint == null)
				return false;
			
			return this.AbsoluteNumber.Equals(otherPoint.AbsoluteNumber);
		}

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
		public override int GetHashCode()
		{
			return this.AbsoluteNumber;
		}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
		public override string ToString()
		{
			return this.filename + " covered by " + this.AbsoluteNumber + " at " + this.lineNumber; 
		}

	}
}