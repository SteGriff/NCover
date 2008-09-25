using System;
using System.Collections;

namespace SharpCover
{
	[NoInstrument()]
	public sealed class Results
	{
		private Results()
		{
		}

		static Results()
		{
			results = new Hashtable();
		}

		private static Hashtable results;

		public static void AddReport(string reportname, string outputfile)
		{
			ResultLogger logger = new ResultLogger(reportname, outputfile);
			results[reportname] = logger;
		}

		/// <summary>
		/// Indicates that a coverage point has been reached relating to the specific report.
		/// </summary>
		public static bool Add(string reportname, string outputfile, int point)
		{
			bool result = true;
			
			if(results[reportname] == null)
				Results.AddReport(reportname, outputfile);
	
			ResultLogger logger = (ResultLogger)results[reportname];
			
			try
			{
				logger.AddPoint(point);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		#region Testing
		/// <summary>
		/// For testing purposes only...
		/// </summary>
		public static void Reset()
		{
			results.Clear();
		}
		#endregion
	}
}