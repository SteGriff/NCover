using System.Diagnostics;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using SharpCover.Logging;

	[TaskName("sharpcover")]
	public class SharpCoverTask : Task
	{
		private SharpCover.Actions.ISharpCoverAction action = new SharpCover.Actions.SharpCoverAction();
		private FileSet files;
		
		public SharpCover.Actions.ISharpCoverAction Action
		{
			get{return this.action;}
			set{this.action = value;}
		}

		[BuildElementAttribute("instrument", Required=true)]
		public FileSet Files
		{
			get { return files; }
			set { files = value; }
		}
		
		[TaskAttribute("reportName", Required=false)]
		public string ReportName
		{
			get { return action.Settings.ReportName; }
			set { action.Settings.ReportName = value; }
		}

		[TaskAttribute("reportDir", Required=false)]
		public string ReportDir
		{
			get { return action.Settings.ReportDir; }
			set { action.Settings.ReportDir = value; }
		}

		public void Run()
		{
			this.ExecuteTask();
		}

		protected override void ExecuteTask()
		{
			NAntLogger.AddNAntListener(this);

			Trace.WriteLineIf(Logger.OutputType.TraceInfo, "SharpCover version " + this.GetType().Assembly.GetName().Version.ToString());
			Trace.WriteLineIf(Logger.OutputType.TraceInfo, "Instrumenting for report " + action.Settings.ReportName);
	
			this.action.Settings.ReportName = this.ReportName;
			this.action.Settings.ReportDir = this.ReportDir;
			
			if(this.Project != null)
				this.action.Settings.BaseDir = this.Project.BaseDirectory;

			files.FailOnEmpty = true;
			files.Scan();
			this.action.Filenames = files.FileNames;
			
			this.action.Execute();

			NAntLogger.RemoveNAntListener();
		}
	}