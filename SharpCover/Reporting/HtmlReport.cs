using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using SharpCover.Logging;
using SharpCover.Utilities;

namespace SharpCover.Reporting
{
    /// <summary>
    /// 
    /// </summary>
	public sealed class HtmlReport
	{
		private HtmlReport()
		{
		}

        /// <summary>
        /// Generates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="report">The report.</param>
		public static void Generate(ReportSettings settings, Report report)
		{			
			// Load the transform
			XslCompiledTransform transform = LoadTransform("SharpCover.Report.xslt");
			
			// Save the required stylesheet and images to the output folder
			WriteResource(settings.CssFilename, "SharpCover.SharpCover.css", ResourceType.Text);
			WriteResource(settings.GetFilename("SharpCover", ".gif"), "SharpCover.SharpCover.gif", ResourceType.Binary);

			// Convert the report to a format we can transform
			XPathDocument doc = ConvertReportToXPathDocument(report);
			
			// Do the transform and write the results to disk
			WriteReport(transform, doc, settings.ReportFilename);
		}

        private static void WriteReport(XslCompiledTransform transform, XPathDocument doc, string filename)
		{
			FileStream output = new FileStream(filename, FileMode.Create);
			
            using (XmlTextWriter writer = new XmlTextWriter(output, Encoding.UTF8))
			{
				transform.Transform(doc.CreateNavigator(), writer);
			}
		}

        private static XslCompiledTransform LoadTransform(string resourcename)
		{
            XslCompiledTransform transform = new XslCompiledTransform();

			using (Stream stream = ResourceManager.GetResource(resourcename))
			{
				transform.Load(new XmlTextReader(stream));
			}

			return transform;
		}

		private static XPathDocument ConvertReportToXPathDocument(Report report)
		{
			MemoryStream ms = new MemoryStream();
			SharpCover.Utilities.Serialization.ToXml(ms, report, false);
			ms.Seek(0,0);

			XPathDocument doc = new XPathDocument(ms);
			
			ms.Close();

			return doc;
		}

		private static void WriteResource(string filename, string resource, ResourceType type)
		{
			if(!File.Exists(filename))
			{
				FileStream fs = new FileStream(filename, FileMode.CreateNew);
				ResourceManager.WriteResourceToStream(fs, resource, type);	
				fs.Close();
			}
			else
			{
				Trace.WriteLineIf(Logger.OutputType.TraceVerbose, String.Format("File {0} already exists on disk so will be skipped", filename));
			}		
		}
	}
}