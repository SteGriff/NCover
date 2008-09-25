using System.Diagnostics;
using NAnt.Core;

namespace SharpCover.Logging
{
	public class NAntLogger : Logger
	{
		public static void RemoveNAntListener()
		{
			if(Trace.Listeners["NAnt"] != null)
				Trace.Listeners.Remove("NAnt");
		}

		public static void AddNAntListener(Task task)
		{
			try
			{
				if(task.Verbose)
					OutputType.Level = TraceLevel.Verbose;
			}
			catch
			{
				// Happens because NAnt throws an exception!
			}

			if(Trace.Listeners["NAnt"] != null)
				Trace.Listeners.Remove("NAnt");

			Trace.Listeners.Add(new NAntTraceListener(task));
		}
	}
}