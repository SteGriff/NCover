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

			if(!File.Exists(this.destinationFilename))
			{
				using (TextWriter writer = new StreamWriter(this.destinationFilename, false))
				{
					writer.WriteLine("<?xml version='1.0' encoding='utf-8'?><Coverage  xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' Type='actualCoverage'><CoveragePoints>");
					writer.Flush();
				}
			}
		}

		private Hashtable points;
		private string destinationFilename = null;

		public void AddPoint(int pointid)
		{
			if(points[pointid] != null && (bool)points[pointid] == true)
				return;

			using (TextWriter writer = new StreamWriter(destinationFilename, true))
			{
				writer.WriteLine(String.Format("\t<CoveragePoint AbsoluteNumber='{0}' />", pointid));
				writer.Flush();
			}

			points[pointid] = true;	
		}
	}
}