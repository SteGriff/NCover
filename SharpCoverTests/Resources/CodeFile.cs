using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using SharpCover;
using SharpCover.Parsing;
using SharpCover.Parsing.CSharp;

public class TestParser : IParse
	{
		public TestParser(ReportSettings Settings)
		{
			this.settings = Settings;
			this.coveragepoints = new ArrayList();
			
			AddCoveragePointDelegate addpointdel = new AddCoveragePointDelegate(this.AddCoveragePoint);

			this.matchers = new ArrayList();
			this.matchers.Add(new StatementMatcher(addpointdel));
			this.matchers.Add(new ConditionalMatcher(addpointdel));
			this.matchers.Add(new CatchBlockMatcher(addpointdel));
			this.matchers.Add(new MethodMatcher(addpointdel));
			this.matchers.Add(new FlattenMatcher(addpointdel));
			this.matchers.Add(new UnflattenMatcher(addpointdel));
		}
		
		private static int baseCoveragePoint = 0;
		private readonly static Regex filenamespaceRegex = new Regex(@"namespace\s+([^\s]+)\s*{", RegexOptions.Compiled);
		
		private string source;
		private string filename;
		private ArrayList matchers;
		private	ArrayList coveragepoints;
		private ReportSettings settings;
		private MatchCollection namespaces;
		
		public ArrayList Matchers
		{
			get{return this.matchers;}
		}

		public CoveragePoint[] CoveragePoints
		{
			get{return (CoveragePoint[])coveragepoints.ToArray(typeof(CoveragePoint));}
		}

	public bool Accept(string filename)
	{
		return false;
	}

	public string Parse(string filename)
		{
			string file;
			this.filename = filename;
			using(StreamReader reader = new StreamReader(filename))
			{
				file = reader.ReadToEnd();
			}

			return ParseString(file);
		}

		public string ParseString(string source)
		{
			this.source = source;
			this.coveragepoints = new ArrayList();
			
			Matcher.Comments = new Comments(this.source);

			this.namespaces = filenamespaceRegex.Matches(this.source);
			
			foreach(Matcher matcher in this.matchers)
			{
				this.source = matcher.Replace(this.source);
			}
			
			baseCoveragePoint += this.coveragepoints.Count;

			return this.source;
		}
		
		private string AddCoveragePoint(int Index)
		{
			CoveragePoint point = new CoveragePoint(filename, GetNamespace(Index), coveragepoints.Count + 1, baseCoveragePoint, false);
			coveragepoints.Add(point);

			point.LineNumber = IndexToLineNumber(Index);

			return point.GetCoverageCode(this.settings);
		}

		private string GetNamespace(int Line)
		{			
			int length = this.namespaces.Count;
			for(int i=0; i < length; i++)
			{
				if(i == length - 1 || (this.namespaces[i].Index <= Line && this.namespaces[i + 1].Index >= Line))
					return this.namespaces[i].Groups[1].Value;
			}

			return "Default";
		}
		
		private int IndexToLineNumber(int index)
		{
			int lineNumber = 1;
			for(int i = 0; i <= index; i++)
			{
				if (this.source[i] == '\n')
					lineNumber++;
			}

			return lineNumber;
		}
	}

namespace SharpCover
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
	public sealed class TestResults
	{
		private TestResults()
		{
		}

		static TestResults()
		{
			results = new Hashtable();
		}

		private static Hashtable results;

		public static void AddReport(string reportname, string outputfile)
		{
			ResultLogger logger = new ResultLogger(reportname, outputfile);
			results[reportname] = logger;
		}

		///<summary>
		/// Instrumented code calls here to notify that a coverage point has been called.
		/// Must always return true.
		///</summary>
		public static bool Add(string reportname, string outputfile, int point)
		{
			bool result = true;
			
			if(results[reportname] == null)
				Results.AddReport(reportname, outputfile);
	
			ResultLogger logger = (ResultLogger)results[reportname];
			
			try
			{
				logger.AddPoint(point);
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}