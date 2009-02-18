using System;

namespace SharpCover.Parsing
{
    /// <summary>
    /// 
    /// </summary>
	public class Insert
	{
		private int insert;
		private readonly string body;

        /// <summary>
        /// Initializes a new instance of the <see cref="Insert"/> class.
        /// </summary>
        /// <param name="Insert">The insert.</param>
        /// <param name="Body">The body.</param>
		public Insert(int Insert, string Body)
		{
			this.InsertPoint = Insert;
			this.body = Body;
		}

        /// <summary>
        /// Gets the selected area.
        /// </summary>
        /// <value>The selected area.</value>
		public string SelectedArea
		{
			get{return this.body;}
		}

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
		public int Length
		{
			get { return body.Length; }
		}

        /// <summary>
        /// Gets or sets the insert point.
        /// </summary>
        /// <value>The insert point.</value>
		public int InsertPoint
		{
			get { return this.insert; }
			set { 
				if (value < 0)
					throw new ApplicationException("Invalid insertion value (negative) " + value);
				insert = value; 
			}
		}
	}
}