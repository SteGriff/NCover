using System.Diagnostics;
using NAnt.Core;

namespace SharpCover.Logging
{
	public class NAntTraceListener : TraceListener
	{
		public NAntTraceListener(Task task) : base("NAnt")
		{
			this.task = task;
		}

		public NAntTraceListener(Task task, string name) : base(name)
		{
			this.task = task;
		}

		private Task task;

		public override void Write(string str)
		{
				this.task.Log(Level.Info, str);
		}

		public override void Write(object obj)
		{
			Write(obj.ToString());
		}

		public override void Write(string str, string category)
		{
				this.task.Log(Level.Info, category + ":\t" + str);
		}

		public override void Write(object obj, string category)
		{
			Write(obj.ToString(), category);
		}

		public override void WriteLine(string str)
		{
				this.task.Log(Level.Info, str);
		}

		public override void WriteLine(object obj)
		{
			WriteLine(obj.ToString());
		}

		public override void WriteLine(string str, string category)
		{
				this.task.Log(Level.Info, category + ":\t" + str);
		}

		public override void WriteLine(object obj, string category)
		{
			WriteLine(obj.ToString(), category);
		}		

		public override void Flush()
		{
		}

		public override void Fail(string error)
		{
				this.task.Log(Level.Warning, "An error has occurred:\t" + error);
		}

		public override void Fail(string error, string description)
		{
			Fail(error);
				this.task.Log(Level.Warning, description);
		}
	}
}