using System;
using System.Diagnostics;

namespace SharpCover.Logging
{
	public class MockTraceListener : TraceListener
	{
		public MockTraceListener()
		{
		}

		private int numwrites = 0;
		private int numwritelines = 0;
		private int numfails = 0;
		private int numflushes = 0;

		public int NumWrites
		{
			get{return this.numwrites;}
		}

		public int NumWriteLines
		{
			get{return this.numwritelines;}
		}

		public int NumFails
		{
			get{return this.numfails;}
		}

		public int NumFlushes
		{
			get{return this.numflushes;}
		}

		public override void Write(string str)
		{
			this.numwrites ++;
		}

		public override void Write(object obj)
		{
			Write(obj.ToString());
		}

		public override void Write(string str, string category)
		{
			this.numwrites ++;
		}

		public override void Write(object obj, string category)
		{
			Write(obj.ToString(), category);
		}

		public override void WriteLine(string str)
		{
			this.numwritelines++;
		}

		public override void WriteLine(object obj)
		{
			WriteLine(obj.ToString());
		}

		public override void WriteLine(string str, string category)
		{
			this.numwritelines++;
		}

		public override void WriteLine(object obj, string category)
		{
			WriteLine(obj.ToString(), category);
		}		

		public override void Flush()
		{
			this.numflushes++;
		}

		public override void Fail(string error)
		{
			this.numfails ++;
		}

		public override void Fail(string error, string description)
		{
			Fail(error);
		}
	}
}