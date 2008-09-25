using System;
using System.Diagnostics;

namespace SharpCover.Logging
{
	public class ConsoleTraceListener : TraceListener
	{
		public ConsoleTraceListener() : base("Console")
		{
		}

		public ConsoleTraceListener(string name) : base(name)
		{
		}

		public override void Write(string str)
		{
			Console.Out.Write(str);
		}

		public override void Write(object obj)
		{
			Write(obj.ToString());
		}

		public override void Write(string str, string category)
		{
			Console.Out.Write(category + ":\t" + str);
		}

		public override void Write(object obj, string category)
		{
			Write(obj.ToString(), category);
		}

		public override void WriteLine(string str)
		{
			Console.Out.WriteLine(str);
		}

		public override void WriteLine(object obj)
		{
			WriteLine(obj.ToString());
		}

		public override void WriteLine(string str, string category)
		{
			Console.Out.WriteLine(category + ":\t" + str);
		}

		public override void WriteLine(object obj, string category)
		{
			WriteLine(obj.ToString(), category);
		}		

		public override void Flush()
		{
			Console.Out.Flush();
		}

		public override void Fail(string error)
		{
			WriteLine("An error has occurred:\t" + error);
		}

		public override void Fail(string error, string description)
		{
			Fail(error);
			WriteLine(description);
		}
	}
}