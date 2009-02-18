using System;
using System.IO;
using System.Text;
using System.Xml;

namespace SharpCover.Parsing.ProjectFiles
{
	/// <summary>
	/// Finds project files and adds a reference to SharpCover into them.
	/// </summary>
	public class ProjectFileParser : IParse
	{

        private CoveragePoint coveragePoint;

		#region IParse Members

        /// <summary>
        /// Returns true if this parser can parse this type of file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
		public bool Accept(string filename)
		{
			return filename.EndsWith(".csproj") || filename.EndsWith(".vbproj");
		}

        /// <summary>
        /// Parses the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
		public string Parse(string filename)
		{
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("def", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlElement assemblyName = (XmlElement)doc.DocumentElement.SelectSingleNode("def:PropertyGroup/def:AssemblyName", nsmgr);

            if (string.Compare(assemblyName.Value, typeof(Results).Assembly.GetName().Name, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // don't extend yourself
                return null;
            }

            string assemblyReference = typeof(Results).Assembly.FullName;
            XmlElement itemGroup = (XmlElement)doc.DocumentElement.SelectSingleNode("def:ItemGroup", nsmgr);
            XmlElement reference = (XmlElement)itemGroup.SelectSingleNode(string.Format("def:Reference[@Include='{0}']", assemblyReference), nsmgr);
            string fileContent = null;

            if (reference == null)
            {
                reference = doc.CreateElement("Reference");

                XmlAttribute attr = doc.CreateAttribute("Include");
                attr.Value = assemblyReference;

                reference.Attributes.Append(attr);

                XmlElement specificVersion = doc.CreateElement("SpecificVersion");
                XmlText specificVersionValue = doc.CreateTextNode(bool.FalseString);
                specificVersion.AppendChild(specificVersionValue);
                reference.AppendChild(specificVersion);

                XmlElement hintPath = doc.CreateElement("HintPath");
                XmlText hintPathValue = doc.CreateTextNode(typeof(Results).Assembly.Location);
                hintPath.AppendChild(hintPathValue);
                reference.AppendChild(hintPath);

                itemGroup.AppendChild(reference);

                using (MemoryStream ms = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(ms, Encoding.UTF8);

                    doc.Save(writer);
                    ms.Seek(0, SeekOrigin.Begin);

                    using (StreamReader sr = new StreamReader(ms, Encoding.UTF8, false))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                }

                // HACK: remove empty xmlns attribute!
                fileContent = fileContent.Replace(" xmlns=\"\"", string.Empty);
            }

            coveragePoint = new CoveragePoint(filename, null, -1, -1, false);

            return fileContent;
		}

        /// <summary>
        /// Gets the coverage points.
        /// </summary>
        /// <value>The coverage points.</value>
		public CoveragePoint[] CoveragePoints
		{
			get
			{
                if (coveragePoint == null)
                {
                    return null;
                }

                return new CoveragePoint[] { coveragePoint };
			}
		}

		#endregion

	}
}
