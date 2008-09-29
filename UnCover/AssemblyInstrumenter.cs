using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UnCover
{
    class Il
    {
        private readonly string m_AssemblyLocation;
        
        public Il(String location)
        {
            m_AssemblyLocation = location;
        }

        public string GetFilename()
        {
            return Path.GetFileName(m_AssemblyLocation);
        }
        public string GetFilenameWithoutExt()
        {
            return Path.GetFileNameWithoutExtension(m_AssemblyLocation);
        }

        public string CompileFile(string target)
        {
            var cmdline = string.Format(@" /QUIET {0} /OUT=""{1}"" /RESOURCE=""{2}"" ""{3}""", 
                (target.ToUpper().EndsWith(".DLL")? "/DLL" : "/EXE"), 
                target, 
                Path.Combine(tempDir, GetFilenameWithoutExt() + ".res"), 
                Path.Combine(tempDir, GetFilenameWithoutExt() + @".il"));
            Console.WriteLine(@"C:\Windows\Microsoft.NET\Framework\v2.0.50727\ilasm.exe " + cmdline);
            var processStartInfo = new ProcessStartInfo(@"C:\Windows\Microsoft.NET\Framework\v2.0.50727\ilasm.exe", cmdline)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            var process = Process.Start(processStartInfo);

            if (process != null)
            {
                process.WaitForExit();
            }
            return target;
        }

        public string DecompileFile()
        {
            var cmdline = string.Format(@"""{0}"" /TEXT /SOURCE /TYPELIST /LINENUM /NOBAR /ALL /OUT=""{1}""",
                m_AssemblyLocation + ".uninstrumented", 
                Path.Combine(tempDir,GetFilenameWithoutExt() + @".il"));
            Console.WriteLine(@"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\ildasm.exe " + cmdline);
            var processStartInfo = new ProcessStartInfo(@"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\ildasm.exe", cmdline)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                WorkingDirectory = tempDir,
                CreateNoWindow = true,
            };
            Process process = Process.Start(processStartInfo);

            if (process != null)
            {
                process.WaitForExit();
            }

            // *********** DISASSEMBLY COMPLETE ***********************
            // WARNING: Created Win32 resource file c:\uncover\Simple.res
            //TODO: use il file to find additional resource files as above.

            return Path.Combine(tempDir, GetFilenameWithoutExt() + ".il");
        }

        public readonly string tempDir = Path.GetTempPath();
    }

    public class AssemblyInstrumenter : IDisposable
    {
        private readonly string m_AssemblyLocation;
        private readonly bool m_RollbackOnDispose;
        private const string EXTENTION = ".uninstrumented";

        public AssemblyInstrumenter()
        {
            m_RollbackOnDispose = false;
        }

        public AssemblyInstrumenter(string assemblyLocation): this(assemblyLocation, true) {}

        public AssemblyInstrumenter(string assemblyLocation, bool rollbackOnDispose)
        {
            m_AssemblyLocation = assemblyLocation;
            m_RollbackOnDispose = rollbackOnDispose;
            if (AlreadyInstrumented(m_AssemblyLocation))
            {
                File.Delete(m_AssemblyLocation + EXTENTION);
            }
            
            BackupUninstrumentedAssembly();

            var il = new Il(assemblyLocation);
            string file = il.DecompileFile();


            string contents = File.ReadAllText(file);
            string result = InstrumentIl(contents);
            string coverageLocation = assemblyLocation + ".coverage.xml";
            Console.WriteLine("Coverage will be found here: " + coverageLocation);
            File.WriteAllText(file, result + ilInjection(coverageLocation)); 
            il.CompileFile(assemblyLocation);
        }

        private void BackupUninstrumentedAssembly()
        {
            Console.WriteLine("Instrumenting " + m_AssemblyLocation);
            File.Move(m_AssemblyLocation, m_AssemblyLocation + EXTENTION);
        }

        public static void RestoreUninstrumentedAssembly(string location)
        {
            Console.WriteLine("Looking to uninstrument dll @ " + location);
            if (File.Exists(location + EXTENTION))
            {
                File.Delete(location);
                File.Move(location + EXTENTION, location);
            }
        }

        private static bool AlreadyInstrumented(string assemblyLocation)
        {
            return File.Exists(assemblyLocation + EXTENTION);
        }

        public string InstrumentIl(string contents)
        {
            var regex = new Regex(@"(\s)(ret|brtrue.s\s+\w+)", RegexOptions.Multiline);
            return regex.Replace(contents, new MatchEvaluator(MatchEval));
        }

        private int m_Point;
        private string MatchEval(Match match)
        {
           if (match.Groups[2].Value.Equals("ret"))
           {
               return match.Groups[1].Value + "ldc.i4.s   " + (m_Point++) + "\r\ncall void CoverageResults::Add(int32)\r\n" + match.Groups[2].Value;           
           }
           return match.Groups[1].Value + match.Groups[2].Value + "\r\nldc.i4.s   " + (m_Point++) + "\r\ncall void CoverageResults::Add(int32)";
        }

        //assume dll

        public string ilInjection(string filename)
        {
                return
                    @"


.class /*02000003*/ public auto ansi sealed CoverageResults
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000001*/ private static initonly class [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/ points
  .field /*04000002*/ private static initonly string destinationFilename
  .method /*06000003*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x208c
    // Code size       88 (0x58)
    .maxstack  2
    .locals /*11000001*/ init ([0] bool CS$4$0000)
    .line 21,21 : 9,60 ''
//000017:     }
//000018: 
//000019:     public sealed class CoverageResults
//000020:     {
//000021:         static readonly Hashtable points = new Hashtable();
    IL_0000:  /* 73   | (0A)000012       */ newobj     instance void [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/::.ctor() /* 0A000012 */
    IL_0005:  /* 80   | (04)000001       */ stsfld     class [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/ CoverageResults/*02000003*/::points /* 04000001 */
                                    ldstr      """ + filename.Replace(@"\", @"\\") + @"""
                                    ldstr      """ + filename.Replace(@"\", @"\\") + @"""
                                    call       void [mscorlib]System.Console::WriteLine(string)
    IL_002d:  /* 80   | (04)000002       */ stsfld     string CoverageResults/*02000003*/::destinationFilename /* 04000002 */
    .line 25,25 : 9,10 ''
//000023:             
//000024:         static CoverageResults()
//000025:         {
    IL_0032:  /* 00   |                  */ nop
    .line 26,26 : 13,50 ''
//000026:             if (File.Exists(destinationFilename)) 
    IL_0033:  /* 7E   | (04)000002       */ ldsfld     string CoverageResults/*02000003*/::destinationFilename /* 04000002 */
    IL_0038:  /* 28   | (0A)000018       */ call       bool [mscorlib/*23000001*/]System.IO.File/*01000019*/::Exists(string) /* 0A000018 */
    IL_003d:  /* 16   |                  */ ldc.i4.0
    IL_003e:  /* FE01 |                  */ ceq
    IL_0040:  /* 0A   |                  */ stloc.0
    IL_0041:  /* 06   |                  */ ldloc.0
    IL_0042:  /* 2D   | 02               */ brtrue.s   IL_0046

    .line 27,27 : 17,24 ''
//000027:                 return;
    IL_0044:  /* 2B   | 11               */ br.s       IL_0057

    .line 29,34 : 13,46 ''
//000028: 
//000029:             File.WriteAllText(destinationFilename, @""<?xml version='1.0' encoding='utf-8'?>
    IL_0046:  /* 7E   | (04)000002       */ ldsfld     string CoverageResults/*02000003*/::destinationFilename /* 04000002 */
    IL_004b:  /* 72   | (70)00002D       */ ldstr      ""<\?xml version='1.0' encoding='utf-8'\?>\r\n\r\n<Coverag""
    + ""e  \r\n    xmlns:xsd='http://www.w3.org/2001/XMLSchema' \r\n    xmlns:xsi='http://www.w3.org/2001/XMLSche""
    + ""ma-instance' \r\n    Type='actualCoverage' ExpectedPoints='" +
                    m_Point +
                    @"'><CoveragePoints>"" /* 7000002D */
    IL_0050:  /* 28   | (0A)000019       */ call       void [mscorlib/*23000001*/]System.IO.File/*01000019*/::WriteAllText(string,
                                                                                                                           string) /* 0A000019 */
    IL_0055:  /* 00   |                  */ nop
    .line 35,35 : 9,10 ''
//000030: 
//000031: <Coverage  
//000032:     xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
//000033:     xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' 
//000034:     Type='actualCoverage'><CoveragePoints>"");
//000035:         }
    IL_0056:  /* 00   |                  */ nop
    IL_0057:  /* 2A   |                  */ ret
  } // end of method CoverageResults::.cctor

  .method /*06000004*/ public hidebysig static 
          void  Add(int32 pointid) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x20f0
    // Code size       79 (0x4f)
    .maxstack  3
    .locals /*11000002*/ init ([0] string 'record',
             [1] bool CS$4$0000)
    .line 41,41 : 9,10 ''
//000036: 
//000037:         /// <summary>
//000038:         /// Indicates that a coverage m_Point has been reached.
//000039:         /// </summary>
//000040:         public static void Add(int pointid)
//000041:         {
    IL_0000:  /* 00   |                  */ nop
    .line 42,42 : 13,45 ''
//000042:             if (points.ContainsKey(pointid)) 
    IL_0001:  /* 7E   | (04)000001       */ ldsfld     class [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/ CoverageResults/*02000003*/::points /* 04000001 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 8C   | (01)00001A       */ box        [mscorlib/*23000001*/]System.Int32/*0100001A*/
    IL_000c:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/::ContainsKey(object) /* 0A00001A */
    IL_0011:  /* 16   |                  */ ldc.i4.0
    IL_0012:  /* FE01 |                  */ ceq
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 07   |                  */ ldloc.1
    IL_0016:  /* 2D   | 02               */ brtrue.s   IL_001a

    .line 43,43 : 17,24 ''
//000043:                 return;
    IL_0018:  /* 2B   | 34               */ br.s       IL_004e

    .line 45,45 : 13,93 ''
//000044: 
//000045:             var record = String.Format(""\t<CoveragePoint AbsoluteNumber='{0}' />"", pointid);
    IL_001a:  /* 72   | (70)0001D0       */ ldstr      ""\r\n\t<CoveragePoint AbsoluteNumber='{0}' />"" /* 700001D0 */
    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 8C   | (01)00001A       */ box        [mscorlib/*23000001*/]System.Int32/*0100001A*/
    IL_0025:  /* 28   | (0A)00001B       */ call       string [mscorlib/*23000001*/]System.String/*01000018*/::Format(string,
                                                                                                                      object) /* 0A00001B */
    IL_002a:  /* 0A   |                  */ stloc.0
    .line 46,46 : 13,61 ''
//000046:             File.AppendAllText(destinationFilename, record);
    IL_002b:  /* 7E   | (04)000002       */ ldsfld     string CoverageResults/*02000003*/::destinationFilename /* 04000002 */
    IL_0030:  /* 06   |                  */ ldloc.0
    IL_0031:  /* 28   | (0A)00001C       */ call       void [mscorlib/*23000001*/]System.IO.File/*01000019*/::AppendAllText(string,
                                                                                                                            string) /* 0A00001C */
    IL_0036:  /* 00   |                  */ nop
    .line 47,47 : 13,36 ''
//000047:             points[pointid] = true;
    IL_0037:  /* 7E   | (04)000001       */ ldsfld     class [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/ CoverageResults/*02000003*/::points /* 04000001 */
    IL_003c:  /* 02   |                  */ ldarg.0
    IL_003d:  /* 8C   | (01)00001A       */ box        [mscorlib/*23000001*/]System.Int32/*0100001A*/
    IL_0042:  /* 17   |                  */ ldc.i4.1
    IL_0043:  /* 8C   | (01)00001B       */ box        [mscorlib/*23000001*/]System.Boolean/*0100001B*/
    IL_0048:  /* 6F   | (0A)00001D       */ callvirt   instance void [mscorlib/*23000001*/]System.Collections.Hashtable/*01000002*/::set_Item(object,
                                                                                                                                              object) /* 0A00001D */
    IL_004d:  /* 00   |                  */ nop
    .line 48,48 : 9,10 ''
//000048:         }
    IL_004e:  /* 2A   |                  */ ret
  } // end of method CoverageResults::Add

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000011       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000011 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CoverageResults::.ctor

} // end of class CoverageResults
";
            
        }

        public void Dispose()
        {
            if (m_RollbackOnDispose)
            {
                RestoreUninstrumentedAssembly(m_AssemblyLocation);
            }
        }
    }
}
