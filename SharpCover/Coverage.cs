using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using SharpCover.Logging;

namespace SharpCover
{
	/// <summary>
	/// Holds the raw data for what coverage points have been hit / missed.
	/// </summary>
	public class Coverage
	{	
		public Coverage()
		{
			this.settings = new ReportSettings();
		}

		public Coverage(ReportSettings settings, CoveragePoint[] points)
		{
			this.settings = settings;
			this.CoveragePoints = points;
		}

		private ReportSettings settings;

		public ReportSettings Settings
		{
			get{return this.settings;}
			set{this.settings = value;}
		}
		
		public CoveragePoint[] CoveragePoints;

		/// <summary>
		/// Saves the expected coverage
		/// </summary>
		public static void SaveCoverage(Stream outputstream, Coverage coverage)
		{
			SharpCover.Utilities.Serialization.ToXml(outputstream, coverage, true);
		}			

		public static Coverage LoadCoverage(Stream expected, Stream actual)
		{
			var coverage = (Coverage)Utilities.Serialization.FromXml(expected, typeof(Coverage));
			var actualcoverage = (Coverage)Utilities.Serialization.FromXml(actual, typeof(Coverage));
			
			expected.Close();
			actual.Close();

			SetMisses(coverage, actualcoverage);

			return coverage;
		}

		public static Stream FixActualFile(Stream actual)
		{
			MemoryStream ms = new MemoryStream();
			string contents = null;
			
			using (StreamReader reader = new StreamReader(actual))
			{
				contents = reader.ReadToEnd().Trim();
				if (!contents.EndsWith("</Coverage>"))
					contents += "</CoveragePoints></Coverage>";
			}

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(contents);

			doc.Save(ms);
			ms.Seek(0,0);
			return ms;
		}

		private static void SetMisses(Coverage expectedCoverage, Coverage actualCoverage)
		{
			Trace.WriteLineIf(Logger.OutputType.TraceVerbose, expectedCoverage.CoveragePoints.Length, "Expected Points");
			Trace.WriteLineIf(Logger.OutputType.TraceVerbose, actualCoverage.CoveragePoints.Length, "Actual Points");

			var actualPoints = new Hashtable();
			foreach (CoveragePoint actualPoint in actualCoverage.CoveragePoints)
			{
				actualPoints.Add(actualPoint, actualPoint.Hit);
			}
			
			int hitCount = 0;
			
			foreach (CoveragePoint expected in expectedCoverage.CoveragePoints)
			{
			    if (actualPoints[expected] == null)
			    {
			        continue;
			    }
			    expected.Hit = true;
			    hitCount++;
			}
		}
	}
}