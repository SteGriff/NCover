using System;
using NUnit.Framework;

namespace UnCover
{
    [TestFixture]
    public class AssemblyLoaderTest
    {

        [Test]
        public void CanLoadAnAssemblyFromDisk()
        {
            var testassembly = @"C:\work\simple\Simple\SimpleTests\bin\Debug\SimpleTests.dll";
            using (new AssemblyInstrumenter(@"C:\work\simple\Simple\SimpleTests\bin\Debug\Simple.exe"))
            {
                NUnit.ConsoleRunner.Runner.Main(new[] {testassembly});
            }
        }

        [Test]
        public void InstrumentIlForReturn()
        {
            const string input = @"
IL_001a:  /* 00   |                  */ nop
    .line 15,15 : 9,10 ''
//000015:         }
    IL_001b:  /* 2A   |                  */ ret
  } // end of method Program::Main
";
            const string expected = @"
IL_001a:  /* 00   |                  */ nop
    .line 15,15 : 9,10 ''
//000015:         }
    IL_001b:  /* 2A   |                  */ ldc.i4.s   0
call void CoverageResults::Add(int32)
ret
  } // end of method Program::Main
";

            
            Assert.AreEqual(expected, new AssemblyInstrumenter().InstrumentIl(input));
        }

        [Test]
        public void InstrumentIlForIf()
        {
            const string input = @"
//000010:         {
    IL_0000:  /* 00   |                  */ nop
    .line 11,11 : 13,34 ''
//000011:             if (args.Length == 0)
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 8E   |                  */ ldlen
    IL_0003:  /* 69   |                  */ conv.i4
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* FE01 |                  */ ceq
    IL_0007:  /* 16   |                  */ ldc.i4.0
    IL_0008:  /* FE01 |                  */ ceq
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 06   |                  */ ldloc.0
    IL_000c:  /* 2D   | 0D               */ brtrue.s   IL_001b

    .line 12,12 : 13,14 ''
//000012:             {
    IL_000e:  /* 00   |                  */ nop
";
            const string expected = @"
//000010:         {
    IL_0000:  /* 00   |                  */ nop
    .line 11,11 : 13,34 ''
//000011:             if (args.Length == 0)
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 8E   |                  */ ldlen
    IL_0003:  /* 69   |                  */ conv.i4
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* FE01 |                  */ ceq
    IL_0007:  /* 16   |                  */ ldc.i4.0
    IL_0008:  /* FE01 |                  */ ceq
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 06   |                  */ ldloc.0
    IL_000c:  /* 2D   | 0D               */ brtrue.s   IL_001b
ldc.i4.s   0
call void CoverageResults::Add(int32)

    .line 12,12 : 13,14 ''
//000012:             {
    IL_000e:  /* 00   |                  */ nop
";

            
            Assert.AreEqual(expected, new AssemblyInstrumenter().InstrumentIl(input));
        }


    }

    public class DirectorySwapper : IDisposable
	{
		private readonly string m_SavedDirectoryName;

		public DirectorySwapper( string directoryName )
		{
			m_SavedDirectoryName = Environment.CurrentDirectory;			
			Environment.CurrentDirectory = directoryName;
		}

		public void Dispose()
		{
			Environment.CurrentDirectory = m_SavedDirectoryName;
		}
	}
}
