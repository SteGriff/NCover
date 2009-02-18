using System.Collections.Specialized;

namespace SharpCover.Actions
{
	public class SharpCoverActionMockObject : ISharpCoverAction
	{
		public SharpCoverActionMockObject()
		{
			this.settings = new ReportSettings();
			this.filenames = new StringCollection();
		}

		private ReportSettings settings;
		private int numexecutes = 0;
		private decimal valuetoreturn = 0;
		private StringCollection filenames;

		public decimal ValueToReturn
		{
			get{return this.valuetoreturn;}
			set{this.valuetoreturn = value;}
		}

		public StringCollection Filenames
		{
			get{return this.filenames;}
			set{this.filenames = value;}
		}

		public ReportSettings Settings
		{
			get{ return this.settings;}
		}

		public decimal Execute()
		{
			this.numexecutes ++;
			return this.valuetoreturn;
		}

		public int NumExecutes
		{
			get{return this.numexecutes;}
		}
	}
}