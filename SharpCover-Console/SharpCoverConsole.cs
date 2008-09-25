using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using SharpCover.Utilities;
using SharpCover.Actions;
using SharpCover;
using SharpCover.Logging;

namespace SharpCover.CommandLine
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class SharpCoverConsole
	{
		/// <summary>
		/// The main entry point for the application.
		/// E.g. sharpcover-console /reportname:"SharpCover" /recurse:*.cs
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Logger.AddConsoleListener("SharpCover-Console");

			NameValueCollection parameters = ArgumentParsing.ParseCommandLine(args);
			
			if (parameters[Constants.DEBUG] != null)
				Logger.OutputType.Level = TraceLevel.Verbose;

			SharpCoverAction sharpcoverAction = new SharpCoverAction();
			
			ReportSettings settings = GetSettings(parameters);

			AddFiles(sharpcoverAction.Filenames, settings, parameters[Constants.RECURSE]);
			
			sharpcoverAction.Execute();
		}

		private static void AddFiles(StringCollection files, ReportSettings settings, string dirAndPattern)
		{
			string path = dirAndPattern;

			if(path == null)
				return;

			if(path[0] != '\\')
				path = Path.Combine(settings.BaseDir, dirAndPattern);

			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
			string pattern = Path.GetExtension(path);

			AddFiles(files, settings, dir, pattern);
		}

		private static void AddFiles(StringCollection files, ReportSettings settings, DirectoryInfo dir, string pattern)
		{
			//Add matching files
			foreach (FileInfo file in dir.GetFiles(pattern))
			{
				Trace.WriteLineIf(Logger.OutputType.TraceInfo, file.FullName, "Adding file");
				files.Add(file.FullName);
			}

			//Recurse through sub-directories...
			foreach (DirectoryInfo subdir in dir.GetDirectories())
			{
				AddFiles(files, settings, subdir, pattern);
			}
		}

		private static ReportSettings GetSettings(NameValueCollection nvc)
		{
			ReportSettings settings = new ReportSettings();

			settings.ReportName = nvc[Constants.REPORT_NAME];
			settings.BaseDir = nvc[Constants.BASE_DIR];
			settings.ReportDir = nvc[Constants.REPORT_DIR];
			
			return settings;
		}
	}
}
