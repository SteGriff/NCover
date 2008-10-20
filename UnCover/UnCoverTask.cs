using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace UnCover
{
    
    public class UnCover : Task
    {
        [Required]
        public string[] Assemblies { get; set; }

        [Output]
        public string[] CoverageFiles { get; set; }

        public string ReportName { get; set; }

        public string Action { get; set; }

        public override bool Execute()
        {
            if (ReportName == null)
                ReportName = "Coverage";

            var generatedFiles = new List<string>();
            string[] assemblies = Assemblies;
            if (Action.ToLower().Equals("report"))
            {
                foreach (var assembly in assemblies)
                {
                    string coverageFile = GetCoverageFile(assembly);
                    Console.WriteLine("Checking " + coverageFile);
                    if (File.Exists(coverageFile))
                    {
                        Console.WriteLine("Closing " + coverageFile);

                        var xdoc = new XmlDocument();
                        xdoc.LoadXml(File.ReadAllText(coverageFile) + "</CoveragePoints></Coverage>");
                        var count = long.Parse(xdoc.SelectSingleNode("/Coverage/@ExpectedPoints").Value);

                        var covered = new HashSet<long>();

                        XmlNodeList nodes = xdoc.SelectNodes("/Coverage/CoveragePoints/CoveragePoint/@AbsoluteNumber");
                        if (nodes != null)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                XmlNode item = nodes.Item(i);
                                covered.Add(long.Parse(item.Value));
                            }
                        }

                        double coverage = covered.Count / (double)count;

                        File.AppendAllText(coverageFile, "</CoveragePoints>" + 
@"<Results Percentage='" + coverage + "'></Results>"
+ "</Coverage>");

						Console.WriteLine("##teamcity[progressMessage '"+ ReportName  + "=" + coverage + "']");
                        Console.WriteLine("##teamcity[buildStatisticValue key='" + ReportName + "' value='" + coverage + "']");                        
File.WriteAllText(@"TeamCity-info.xml", @"<build>
   <statisticValue key='graph2Key' value='" + coverage + @"'/>
</build>");
                    }
                    generatedFiles.Add(coverageFile);                    
                    AssemblyInstrumenter.RestoreUninstrumentedAssembly(assembly);
                }
            }
            else
            {
                foreach (var assembly in assemblies)
                {
                    string coverageFile = GetCoverageFile(assembly);
                    if (File.Exists(coverageFile))
                    {
                        File.Delete(coverageFile);
                    }
                    new AssemblyInstrumenter(assembly, false);
                }
            }
            return true;
        }

        private static string GetCoverageFile(string assembly)
        {
			//Path.GetFileNameWithoutExtension(
            return Path.Combine(Path.GetDirectoryName(assembly), assembly + ".coverage.xml");
        }
    }
}
