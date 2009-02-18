using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SharpCover.Parsing.VB
{
    /// <summary>
    /// 
    /// </summary>
	public class Parser : IParse
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="Settings">The settings.</param>
		public Parser(ReportSettings Settings)
		{
			this.settings = Settings;
            this.coveragepoints = new List<CoveragePoint>();
			
			AddCoveragePointDelegate addpointdel = new AddCoveragePointDelegate(this.AddCoveragePoint);

            this.matchers = new List<Matcher>(5);
			//this.matchers.Add(new StatementMatcher(addpointdel));
			this.matchers.Add(new ConditionalMatcher(addpointdel));
			//this.matchers.Add(new CatchBlockMatcher(addpointdel));
			this.matchers.Add(new SubroutineMatcher(addpointdel));
			this.matchers.Add(new FunctionMatcher(addpointdel));
			this.matchers.Add(new FlattenMatcher(addpointdel));
			this.matchers.Add(new UnflattenMatcher(addpointdel));
		}
		
		private static int baseCoveragePoint = 0;
		private readonly static Regex filenamespaceRegex = new Regex(@"Namespace\s+([^\s]+)\s", RegexOptions.Compiled & RegexOptions.Multiline);
		
		private string source;
		private string filename;
        private List<Matcher> matchers;
        private List<CoveragePoint> coveragepoints;
        private ReportSettings settings;
		private MatchCollection namespaces;

        /// <summary>
        /// Gets the matchers.
        /// </summary>
        /// <value>The matchers.</value>
        public List<Matcher> Matchers
		{
			get{return this.matchers;}
		}

        /// <summary>
        /// Gets the coverage points.
        /// </summary>
        /// <value>The coverage points.</value>
		public CoveragePoint[] CoveragePoints
		{
			get{return coveragepoints.ToArray();}
		}

        /// <summary>
        /// Returns true if this parser can parse this type of file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
		public bool Accept(string filename)
		{
			return filename.EndsWith(".vb");
		}

        /// <summary>
        /// Parses the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
		public string Parse(string filename)
		{
			string file;
			this.filename = filename;
			using(StreamReader reader = new StreamReader(filename))
			{
				file = reader.ReadToEnd();
			}

			if(file.IndexOf("[NoInstrument()]") != -1)
				return file;
			else
				return ParseString(file);
		}

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
		public string ParseString(string source)
		{
			this.source = source;
			this.coveragepoints = new List<CoveragePoint>();
			
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

			return GetCoverageCode(this.settings, point);
		}

		private string GetNamespace(int Line)
		{			
			int length = this.namespaces.Count;
			for(int i=0; i < length; i++)
			{
				if(this.namespaces[i].Index <= Line && (i == length - 1 || this.namespaces[i + 1].Index >= Line))
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

        /// <summary>
        /// Gets the coverage code.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
		public string GetCoverageCode(ReportSettings settings, CoveragePoint point)
		{
			string qualifiedCall = "SharpCover.Results.Add(";			
			return String.Format(qualifiedCall + "\"{0}\", \"{1}\", {2})", new object[] { settings.ReportName, settings.ActualFilename.Replace(@"\", @"\\"), point.AbsoluteNumber });
		}
	}
}
