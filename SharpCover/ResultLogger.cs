using System;
using System.Collections;
using System.IO;

namespace SharpCover
{
	[NoInstrument()]
	public class ResultLogger
	{

		public ResultLogger(string reportname, string outputfile)
		{
			this.points = new Hashtable();
			this.destinationFilename = outputfile;

			if (!File.Exists(this.destinationFilename))
			{
				using (TextWriter writer = new StreamWriter(this.destinationFilename, false))
				{
					writer.WriteLine("<?xml version='1.0' encoding='utf-8'?><Coverage  xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' Type='actualCoverage'><CoveragePoints>");
				}
			}
		}

		private Hashtable points;
		private string destinationFilename = null;
        private static readonly object _writeLock = new object();

		public void AddPoint(int pointid)
		{
			if (points.ContainsKey(pointid))
				return;

            lock (_writeLock)
            {
                if (points.ContainsKey(pointid))
                    return;

                using (TextWriter writer = new StreamWriter(destinationFilename, true))
                {
                    writer.WriteLine(String.Format("\t<CoveragePoint AbsoluteNumber='{0}' />", pointid));
                }

                points.Add(pointid, null);
            }
		}
	}
}