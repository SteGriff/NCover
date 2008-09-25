using System;
using NAnt.Core;

namespace SharpCover.Tasks
{
	public class MockTask : Task
	{
		public MockTask()
		{

			this.Project = MockProject.GetMockProject();
		}

		private string lastwrite = "";
		private Level lastlevel;

		public Level LastLevel
		{
			get{return this.lastlevel;}
		}

		public string LastWrite
		{
			get{return this.lastwrite;}
		}

		protected override void ExecuteTask()
		{
		}

		public override string Name 
		{
			get{return "Mock";}
		}

		public override void Log(Level level, string message)
		{
			this.lastwrite = message;
			this.lastlevel = level;
		}
	}
}