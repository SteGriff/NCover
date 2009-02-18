using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace SharpCover
{
	/// <summary>
	/// Additional assertions
	/// </summary>
	public class TestUtilities
	{
		static CSharpCodeProvider cSharpCompiler;
		static ICodeCompiler compiler;
		static CompilerParameters compileParameters;

		static VBCodeProvider vbCompilerProvider;
		static ICodeCompiler vbCompiler;
		static CompilerParameters vbCompileParameters;
		
		static TestUtilities()
		{
			cSharpCompiler = new CSharpCodeProvider();
			compiler = cSharpCompiler.CreateCompiler();
			compileParameters = new CompilerParameters();

			vbCompilerProvider = new VBCodeProvider();
			vbCompiler = vbCompilerProvider.CreateCompiler();
			vbCompileParameters = new CompilerParameters();
		}

		/// <summary>
		/// Checks code compiles.
		/// </summary>
		/// <param name="code">Code must contain only one Type with one method in it</param>
		/// <param name="typeName">Value that the function should return.</param>
		public static object AssertCompilesInCSharp(string code, string typeName)
		{	
			compileParameters.GenerateInMemory = true;
			compileParameters.TreatWarningsAsErrors = true;
			compileParameters.ReferencedAssemblies.Add( typeof( SharpCover.Results ).Assembly.CodeBase.Substring("file:///".Length) );
			
			CompilerResults resultsOfCompile = compiler.CompileAssemblyFromSourceBatch(compileParameters, new string[] { code } );

			foreach (string outline in resultsOfCompile.Output)
			{
				System.Diagnostics.Debug.WriteLine(outline);
			}

			Assert.AreEqual(0, resultsOfCompile.Errors.Count);
			Assert.AreEqual(0, resultsOfCompile.Output.Count);

			Assembly assembly = resultsOfCompile.CompiledAssembly;
			Assert.IsNotNull(assembly);

			Type type = assembly.GetType(typeName);

			return GetNormalMethod(type).Invoke(null, new object[]{});
		}

		/// <summary>
		/// Checks code compiles.
		/// </summary>
		/// <param name="code">Code must contain only one Type with one method in it</param>
		public static object AssertCompilesInVb(string code)
		{	
			vbCompileParameters.GenerateInMemory = true;
			
			vbCompileParameters.TreatWarningsAsErrors = true;
			vbCompileParameters.ReferencedAssemblies.Add( typeof( SharpCover.Results ).Assembly.CodeBase.Substring("file:///".Length) );
			vbCompileParameters.ReferencedAssemblies.Add( typeof( SharpCover.TestUtilities ).Assembly.CodeBase.Substring("file:///".Length) );
			//	vbCompileParameters.ReferencedAssemblies.Add( @"C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Microsoft.VisualBasic.dll" );
			
			//insert magic vb import
			code = @"Imports Microsoft.VisualBasic.Interaction
" + code;
		
			CompilerResults resultsOfCompile = vbCompiler.CompileAssemblyFromSourceBatch(vbCompileParameters, new string[] { code } );

			foreach (string outline in resultsOfCompile.Output)
			{
				System.Diagnostics.Debug.WriteLine(outline);
			}

			Assert.AreEqual(0, resultsOfCompile.Errors.Count, resultsOfCompile.Errors.Count > 0? resultsOfCompile.Errors[0].ErrorText : "");
			Assert.AreEqual(0, resultsOfCompile.Output.Count, resultsOfCompile.Output.Count > 0? resultsOfCompile.Output[0] : "");

			Assembly assembly = resultsOfCompile.CompiledAssembly;
			Assert.IsNotNull(assembly);

			//Assembly types could be > 1 due to new .net2 framework MyApplication,MyProject,MyWebServices

			Assert.IsFalse(assembly.GetTypes()[0].Name.StartsWith("My"),".net2 framework?" )  ;
			Type type = assembly.GetTypes()[0];//assembly.GetType(typeName);

			return GetNormalMethod(type).Invoke(null, new object[]{});
		}

		/// <summary>
		/// Checks code compiles.
		/// </summary>
		/// <param name="code">Code must contain only one Type with one method in it</param>
		/// <param name="expectedValue">Value that the function should return.</param>
		public static void AssertCompilesToCSharpAndReturns(string code, object expectedValue)
		{			
			Assert.AreEqual(expectedValue, AssertCompilesInVb(code));
		}

		private static MethodInfo GetNormalMethod(Type t)
		{
			ArrayList potentials = new ArrayList();
			foreach (MethodInfo methInfo in t.GetMethods())
			{
				if (methInfo.IsSpecialName)
					continue;

				if (methInfo.IsConstructor)
					continue;

				if (methInfo.DeclaringType == typeof(object))
					continue;

				potentials.Add(methInfo);
			}

			Assert.AreEqual(1, potentials.Count);
			return (MethodInfo) potentials[0];
		}

//		public static bool WasHit(CoveragePoint[] points, bool[] results, int lineNumber)
//		{
//			bool resultFound = false;
//			bool result = false;
//
//			foreach (CoveragePoint p in points)
//			{
//				if (p.LineNumber == lineNumber)
//				{
//					if (resultFound)
//					{
//						throw new ApplicationException("Multiple coverage points are on line " + lineNumber  + ". Refactor code...");
//					}
//					resultFound = true;
//					
//					if (p.AbsoluteNumber > results.Length)
//					{
//						return false;
//					}
//
//					result = results[p.AbsoluteNumber - 1];
//				}
//			}
//
//			if (!resultFound)
//			{
//				Assert.Fail("no coverage point found on line " + lineNumber  + ".");
//			}
//			return result;
//		}		 
	}
}
