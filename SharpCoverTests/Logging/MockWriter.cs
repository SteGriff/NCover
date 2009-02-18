using System;
using System.IO;
using System.Text;

namespace SharpCover.Logging
{
	public class MockWriter : TextWriter
	{
		public MockWriter()
		{
		}

		private string lastwrite = "";
		private bool flushcalled = false;

		public bool FlushCalled
		{
			get{return this.flushcalled;}
		}

		public string LastWrite
		{
			get{return this.lastwrite;}
		}

		public override Encoding Encoding
		{
			get{return Encoding.Default;}
		}

		public override void Write(string str)
		{
			this.lastwrite = str;
		}

		public override void WriteLine(string str)
		{
			this.lastwrite = str + Environment.NewLine;
		}

		public override void Flush()
		{
			this.flushcalled = true;
		}
	}
}