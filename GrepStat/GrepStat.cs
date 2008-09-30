using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace GrepStat
{
    /// <summary>
    /// Usage:
    /// 
    /// <UsingTask AssemblyFile="..\tools\GrepStat.dll" TaskName="GrepStat.GrepStat" />
    ///   	<GrepStat Grep="TODO" Stat="Todos" Files="@(Compile)"/>
    /// </summary>
    public class GrepStat : Task
    {
        [Required]
        public string Grep { get; set; }

        [Required]
        public string Stat { get; set; }

        [Output]
        public long Count { get; private set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        public override bool Execute()
        {
            Console.WriteLine("##teamcity[buildStatisticValue key='{0}' value='{1}']", Stat, GetValue());
            return true;
        }

        private long GetValue()
        {
            long results = 0L;
            //Console.WriteLine("GrepStat: # Files" + Files.LongLength);
            foreach (var file in Files)
            {
                string fileName = file.ItemSpec;
                var matches = new Regex(Grep).Matches(File.ReadAllText(fileName));
                //Console.WriteLine("File {0} matches: {1}", fileName, matches.Count);
                results += matches.Count;
            }
            Count = results;
            return Count;
        }
    }
}
