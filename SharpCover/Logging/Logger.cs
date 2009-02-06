using System.Diagnostics;


namespace SharpCover.Logging
{
	public class Logger
	{
		static Logger()
		{
			OutputType.Level = TraceLevel.Info;
		}

		public static readonly TraceSwitch OutputType = new TraceSwitch("Debug", "Sets whether debug messages are written to the trace collection");

		public static void AddConsoleListener(string name)
		{
			if(Trace.Listeners[name] == null)
				Trace.Listeners.Add(new ConsoleTraceListener(name));
		}
	}
}