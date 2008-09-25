using System;

namespace SharpCover.Parsing
{
	public class Insert
	{
		private int insert;
		private readonly string body;

		public Insert(int Insert, string Body)
		{
			this.InsertPoint = Insert;
			this.body = Body;
		}

		public string SelectedArea
		{
			get{return this.body;}
		}

		public int Length
		{
			get { return body.Length; }
		}

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