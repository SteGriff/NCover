using System.Diagnostics;


namespace SharpCover.Logging
{
    /// <summary>
    /// 
    /// </summary>
	public class Logger
	{
        /// <summary>
        /// Initializes the <see cref="Logger"/> class.
        /// </summary>
		static Logger()
		{
			OutputType.Level = TraceLevel.Info;
		}

        /// <summary>
        /// 
        /// </summary>
		public static readonly TraceSwitch OutputType = new TraceSwitch("Debug", "Sets whether debug messages are written to the trace collection");

        /// <summary>
        /// Adds the console listener.
        /// </summary>
        /// <param name="name">The name.</param>
		public static void AddConsoleListener(string name)
		{
			if(Trace.Listeners[name] == null)
				Trace.Listeners.Add(new ConsoleTraceListener(name));
		}
	}
}