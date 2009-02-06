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

		public bool Accept(string filename)
		{
			return filename.EndsWith(".csproj") || filename.EndsWith(".vbproj");
		}

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
            XmlElement existingReference = (XmlElement)itemGroup.SelectSingleNode(string.Format("def:Reference[@Include='{0}']", assemblyReference), nsmgr);
            string fileContent = null;
            string namespaceURI = null;

            if (existingReference == null)
            {
                XmlElement newReference = doc.CreateElement("Reference", namespaceURI);

                XmlAttribute attr = doc.CreateAttribute("Include", namespaceURI);
                attr.Value = assemblyReference;

                newReference.Attributes.Append(attr);

                XmlElement specificVersion = doc.CreateElement("SpecificVersion", namespaceURI);
                XmlText specificVersionValue = doc.CreateTextNode(bool.FalseString);
                specificVersion.AppendChild(specificVersionValue);
                newReference.AppendChild(specificVersion);

                XmlElement hintPath = doc.CreateElement("HintPath", namespaceURI);
                XmlText hintPathValue = doc.CreateTextNode(typeof(Results).Assembly.Location);
                hintPath.AppendChild(hintPathValue);
                newReference.AppendChild(hintPath);

                itemGroup.AppendChild(newReference);

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
            }

            coveragePoint = new CoveragePoint(filename, null, -1, -1, false);

            return fileContent;
		}

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
