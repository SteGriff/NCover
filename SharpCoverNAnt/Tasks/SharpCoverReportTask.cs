using System;
using System.Diagnostics;

using NAnt.Core;
using NAnt.Core.Attributes;

using SharpCover.Logging;

[TaskName("sharpcoverreport")]
public class SharpCoverReportTask : Task
{
	public SharpCoverReportTask() 
	{
	}

	SharpCover.Actions.ISharpCoverAction action = new SharpCover.Actions.SharpCoverReportAction();

	public SharpCover.Actions.ISharpCoverAction Action
	{
		get{return this.action;}
		set{this.action = value;}
	}

	[TaskAttribute("reportDir", Required=false)]
	public string ReportDir
	{
		get { return action.Settings.ReportDir; }
		set { action.Settings.ReportDir = value; }
	}

	[TaskAttribute("reportName", Required=false)]
	public string ReportName
	{
		get { return action.Settings.ReportName; }
		set { action.Settings.ReportName = value; }
	}

	[TaskAttribute("minimumPercentage", Required=false)]
	public decimal Minimum
	{
		get { return action.Settings.MinimumCoverage; }
		set { action.Settings.MinimumCoverage = value; }
	}

	public void Run()
	{
		this.ExecuteTask();
	}

	protected override void ExecuteTask()
	{
		NAntLogger.AddNAntListener(this);
	
		Trace.WriteLineIf(Logger.OutputType.TraceVerbose, "Nant report task entered");
		
		if(this.Project != null)
			action.Settings.BaseDir = this.Project.BaseDirectory;
	
		action.Settings.ReportName = ReportName;
		action.Settings.ReportDir = ReportDir;
		
		decimal testCoverage = action.Execute();

		if (testCoverage < action.Settings.MinimumCoverage)
		{
			throw new BuildException("sharpcoverreport" + String.Format("Test coverage has fallen below minimum standard of {0:P}, coverage now at {1:P}", action.Settings.MinimumCoverage, testCoverage));			
		}
		else
		{
			if (testCoverage < (action.Settings.MinimumCoverage + (decimal)0.1))
			{
				Trace.WriteLineIf(Logger.OutputType.TraceInfo,(String.Format("WARNING: Test coverage within 10% of {0:P} minimum (coverage now {1:P})", action.Settings.MinimumCoverage, testCoverage)));
			}
		}

		NAntLogger.RemoveNAntListener();
	}	
}