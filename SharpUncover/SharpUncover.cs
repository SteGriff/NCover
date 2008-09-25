using System;
using System.Diagnostics;
using System.Collections.Specialized;
using SharpCover;
using SharpCover.Actions;
using SharpCover.Utilities;
using SharpCover.Logging;

namespace SharpUncover
{
    class SharpUncover
    {
        [STAThread]
        static void Main(string[] args)
        {
            Logger.AddConsoleListener("SharpUncover");

            NameValueCollection parameters = ArgumentParsing.ParseCommandLine(args);

            var sharpuncover = new SharpUncoverAction
                                   {
                                       Settings =
                                           {
                                               ReportName = parameters[Constants.REPORT_NAME],
                                               ReportDir = parameters[Constants.REPORT_DIR]
                                           }
                                   };

            //Default
            if (sharpuncover.Settings.ReportDir == null)
            {
                sharpuncover.Settings.ReportDir = Environment.CurrentDirectory;
            }
			
            if (parameters[Constants.DEBUG] != null)
                Logger.OutputType.Level = TraceLevel.Verbose;

            sharpuncover.Execute();
        }
    }
}