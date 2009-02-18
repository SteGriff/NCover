using System.Diagnostics;
using System.IO;
using SharpCover.Logging;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public sealed class HistoryManager
	{
		private HistoryManager()
		{
		}

        /// <summary>
        /// Loads the history.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
		public static History LoadHistory(string filename)
		{
			if(filename == null)
				return null;

			//Load history from disk
			if (!File.Exists(filename))
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "No coverage history at " + filename + " so creating a new one.");
				return new History(filename);
			}
			else
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "Trying to load coverage history at " + filename);				
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{
					Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "Saving coverage history file to " + filename);
					History history = (History)Serialization.FromXml(fs, typeof(History));
					history.Filename = filename;
					return history;
				}
			}
		}

        /// <summary>
        /// Saves the history.
        /// </summary>
        /// <param name="history">The history.</param>
		public static void SaveHistory(History history)
		{
			//Save history to disk
			using (FileStream fs = new FileStream(history.Filename, FileMode.Create))
			{
				Serialization.ToXml(fs, history, true);
			}
		}

		/// <summary>
		/// Updates historical coverage percentages, and returns the up to date history.
		/// </summary>
		public static void Update(History history, Report report)
		{
			Update(history, report.CoveragePercentage);
		}

		private static void Update(History history, decimal percentage)
		{
			//if exactly same coverage percentage as before don't bother updating.
			Event latestEvent = history.Events[0];
			if (latestEvent != null && latestEvent.CoveragePercentage == percentage)
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "Not adding additional historic event as coverage percentage has not changed at " + percentage);
			}
			else
			{
				Event evnt = new Event(percentage);
				history.Events.Add(evnt);
			}
		}
	}
}