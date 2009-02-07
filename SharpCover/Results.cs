using System.Collections;

namespace SharpCover
{
	[NoInstrument()]
	public sealed class Results
	{

		private Results()
		{
		}

		private static Hashtable results = new Hashtable();
        private static readonly object _createLock = new object();

		/// <summary>
		/// Indicates that a coverage point has been reached relating to the specific report.
		/// </summary>
		public static bool Add(string reportname, string outputfile, int point)
		{
			bool result = true;
			
			ResultLogger logger = (ResultLogger)results[reportname];

            if (logger == null)
            {
                lock (_createLock)
                {
                    logger = (ResultLogger)results[reportname];

                    if (logger == null)
                    {
                        logger = new ResultLogger(reportname, outputfile);
                        results[reportname] = logger;
                    }
                }
            }

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