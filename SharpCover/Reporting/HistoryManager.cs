using System;
using System.IO;
using System.Diagnostics;
using SharpCover.Utilities;
using SharpCover.Logging;

namespace SharpCover.Reporting
{
	public sealed class HistoryManager
	{
		private HistoryManager()
		{
		}

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