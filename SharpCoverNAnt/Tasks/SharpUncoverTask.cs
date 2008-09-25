using System;
using System.Diagnostics;

using NAnt.Core;
using NAnt.Core.Types;
using NAnt.Core.Attributes;

using SharpCover;
using SharpCover.Logging;

[TaskName("sharpuncover")]
public class SharpUncoverTask : Task
{
	public SharpUncoverTask()
	{
	}

	SharpCover.Actions.ISharpCoverAction action = new SharpCover.Actions.SharpUncoverAction();

	[TaskAttribute("reportDir", Required=false)]
	public string ReportDir
	{
		get{ return action.Settings.ReportDir; }
		set { action.Settings.ReportDir = value; }
	}

	[TaskAttribute("reportName", Required=false)]
	public string ReportName
	{
		get{return action.Settings.ReportName;}
		set{action.Settings.ReportName = value;}
	}

	public SharpCover.Actions.ISharpCoverAction Action
	{
		get{return this.action;}
		set{this.action = value;}
	}

	public void Run()
	{
		ExecuteTask();
	}

	protected override void ExecuteTask()
	{
		NAntLogger.AddNAntListener(this);

		Trace.WriteLineIf(Logger.OutputType.TraceInfo, " [started]");

		action.Settings.ReportDir = ReportDir;
		action.Settings.ReportName = ReportName;
	
		if(this.Project != null)
			action.Settings.BaseDir = this.Project.BaseDirectory;

		action.Execute();

		Trace.WriteLineIf(Logger.OutputType.TraceInfo, " [finished]");

		NAntLogger.RemoveNAntListener();
	}
}