using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SharpCover.Logging;

namespace SharpCover
{
	/// <summary>
	/// Holds the raw data for what coverage points have been hit / missed.
	/// </summary>
	public class Coverage
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Coverage"/> class.
        /// </summary>
		public Coverage()
		{
			this.settings = new ReportSettings();
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Coverage"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="points">The points.</param>
		public Coverage(ReportSettings settings, CoveragePoint[] points)
		{
			this.settings = settings;
			this.CoveragePoints = points;
		}

		private ReportSettings settings;

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
		public ReportSettings Settings
		{
			get{return this.settings;}
			set{this.settings = value;}
		}

        /// <summary>
        /// 
        /// </summary>
		public CoveragePoint[] CoveragePoints;

		/// <summary>
		/// Saves the expected coverage
		/// </summary>
		public static void SaveCoverage(Stream outputstream, Coverage coverage)
		{
			SharpCover.Utilities.Serialization.ToXml(outputstream, coverage, true);
		}

        /// <summary>
        /// Loads the coverage.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <returns></returns>
		public static Coverage LoadCoverage(Stream expected, Stream actual)
		{
			var coverage = (Coverage)Utilities.Serialization.FromXml(expected, typeof(Coverage));
			var actualcoverage = (Coverage)Utilities.Serialization.FromXml(actual, typeof(Coverage));
			
			expected.Close();
			actual.Close();

			SetMisses(coverage, actualcoverage);

			return coverage;
		}

        /// <summary>
        /// Fixes the actual file.
        /// </summary>
        /// <param name="actual">The actual.</param>
        /// <returns></returns>
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
                if (!actualPoints.ContainsKey(actualPoint) || !(bool)actualPoints[actualPoint])
                {
                    actualPoints[actualPoint] = actualPoint.Hit;
                }
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