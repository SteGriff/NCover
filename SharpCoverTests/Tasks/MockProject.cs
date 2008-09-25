using System;
using System.Xml;
using NAnt.Core;
using SharpCover.Utilities;

namespace SharpCover.Tasks
{
	public sealed class MockProject : Project
	{
		private MockProject(XmlDocument doc) : base(@"c:\sharpcover\SharpCover.build", Level.Info, 0)
		{
		}

		public static Project GetMockProject()
		{
			AppDomain.CurrentDomain.SetupInformation.ConfigurationFile = @"C:\sharpcover\tools\nant\bin\NAnt.exe.config";

			XmlDocument doc = new XmlDocument();
			doc.Load(ResourceManager.GetResource("SharpCover.Resources.buildfile.xml", typeof(MockProject).Assembly));
			
//			XmlNameTable nt = new XmlNameTable();
			
			XmlNamespaceManager _nsMgr = new XmlNamespaceManager(new NameTable());
			_nsMgr.AddNamespace("nant", _nsMgr.DefaultNamespace);

			XmlDocument configdoc = new XmlDocument(_nsMgr.NameTable );
			configdoc.LoadXml(@"<?xml version='1.0'?>
<configuration xmlns='nant'>
    <!-- Leave this alone. Sets up configsectionhandler section -->
    <configSections>
        <section name='nant' type='NAnt.Core.ConfigurationSection, NAnt.Core' />
        <section name='log4net' type='System.Configuration.IgnoreSectionHandler' />
    </configSections>
    <appSettings>
        <!-- Used to indicate the location of the cache folder for shadow files -->
        <add key='shadowfiles.path' value='%temp%\nunit20\ShadowCopyCache' />
        <!-- Used to indicate that NAnt should shadow copy files in a cache folder near the executable -->
        <add key='nant.shadowfiles' value='False' />
        <!-- Used to indicate if cached files should be deleted when done running-->
        <add key='nant.shadowfiles.cleanup' value='False' />
        <!-- To enable internal log4net logging, uncomment the next line -->
        <!-- <add key='log4net.Internal.Debug' value='true'/> -->
    </appSettings>
    <!-- nant config settings -->
    <nant>
        <frameworks>
            <platform name='win32' default='net-1.1'>
                <task-assemblies>
                        <!-- include NAnt task assemblies -->
                        <include name='*Tasks.dll' />
                        <!-- include NAnt test assemblies -->
                        <include name='*Tests.dll' />
                        <!-- include framework-neutral assemblies -->
                        <include name='tasks/*.dll' />
                        <!-- exclude Microsoft.NET specific task assembly -->
                        <exclude name='NAnt.MSNetTasks.dll' />
                        <!-- exclude Microsoft.NET specific test assembly -->
                        <exclude name='NAnt.MSNet.Tests.dll' />
                </task-assemblies>
                <framework 
                    name='net-1.1' 
                    family='net' 
                    version='1.1'
                    description='Microsoft .NET Framework 1.1' 
                    runtimeengine=''
                    sdkdirectory='${path::combine(sdkInstallRoot, ""bin"")}' 
                    frameworkdirectory='${path::combine(installRoot, ""v1.1.4322"")}' 
                    frameworkassemblydirectory='${path::combine(installRoot, ""v1.1.4322"")}'
                    clrversion='1.1.4322'
                    >
                    <task-assemblies>
                        <!-- include .NET specific assemblies -->
                        <include name='tasks/net/*.dll' />
                        <!-- include .NET 1.1 specific assemblies -->
                        <include name='tasks/net/1.1/**/*.dll' />
                        <!-- include Microsoft.NET specific task assembly -->
                        <include name='NAnt.MSNetTasks.dll' />
                        <!-- exclude Microsoft.NET specific test assembly -->
                        <include name='NAnt.MSNet.Tests.dll' />
                    </task-assemblies>
                    <project>
                        <readregistry 
                            property='installRoot'
                            key='SOFTWARE\Microsoft\.NETFramework\InstallRoot'
                            hive='LocalMachine' />
                        <readregistry 
                            property='sdkInstallRoot'
                            key='SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv1.1'
                            hive='LocalMachine'
                            failonerror='false' />
                    </project>
                    <tasks>
                        <task name='csc'>
                            <attribute name='exename'>csc</attribute>
                            <attribute name='supportsnowarnlist'>true</attribute>
                        </task>
                        <task name='vbc'>
                            <attribute name='exename'>vbc</attribute>
                        </task>
                        <task name='jsc'>
                            <attribute name='exename'>jsc</attribute>
                        </task>
                        <task name='vjc'>
                            <attribute name='exename'>vjc</attribute>
                            <attribute name='supportsnowarnlist'>true</attribute>
                        </task>
                        <task name='resgen'>
                            <attribute name='exename'>resgen</attribute>
                        </task>
                        <task name='al'>
                            <attribute name='exename'>al</attribute>
                        </task>
                        <task name='delay-sign'>
                            <attribute name='exename'>sn</attribute>
                        </task>
                        <task name='license'>
                            <attribute name='exename'>lc</attribute>
                        </task>
                        <task name='ilasm'>
                            <attribute name='exename'>ilasm</attribute>
                        </task>
                        <task name='ildasm'>
                            <attribute name='exename'>ildasm</attribute>
                        </task>
                    </tasks>
                </framework>
                <framework 
                    name='net-2.0' 
                    family='net' 
                    version='2.0' 
                    description='Microsoft .NET Framework 2.0 Beta 1' 
                    runtimeengine=''
                    sdkdirectory='${path::combine(sdkInstallRoot, ""bin"")}' 
                    frameworkdirectory='${path::combine(installRoot, ""v2.0.40607"")}' 
                    frameworkassemblydirectory='${path::combine(installRoot, ""v2.0.40607"")}'
                    clrversion='2.0.40607'
                    >
                    <task-assemblies>
                        <!-- include .NET specific assemblies -->
                        <include name='tasks/net/*.dll' />
                        <!-- include .NET 2.0 specific assemblies -->
                        <include name='tasks/net/2.0/**/*.dll' />
                        <!-- include Microsoft.NET specific task assembly -->
                        <include name='NAnt.MSNetTasks.dll' />
                        <!-- exclude Microsoft.NET specific test assembly -->
                        <include name='NAnt.MSNet.Tests.dll' />
                    </task-assemblies>
                    <project>
                        <readregistry
                            property='installRoot'
                            key='SOFTWARE\Microsoft\.NETFramework\InstallRoot'
                            hive='LocalMachine' />
                        <readregistry
                            property='sdkInstallRoot'
                            key='SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0'
                            hive='LocalMachine'
                            failonerror='false' />
                    </project>
                    <tasks>
                        <task name='csc'>
                            <attribute name='exename'>csc</attribute>
                            <attribute name='supportsnowarnlist'>true</attribute>
                            <attribute name='supportswarnaserrorlist'>true</attribute>
                        </task>
                        <task name='vbc'>
                            <attribute name='exename'>vbc</attribute>
                            <attribute name='supportsnowarnlist'>true</attribute>
                            <attribute name='supportswarnaserrorlist'>true</attribute>
                        </task>
                        <task name='jsc'>
                            <attribute name='exename'>jsc</attribute>
                        </task>
                        <task name='vjc'>
                            <attribute name='exename'>vjc</attribute>
                            <attribute name='supportsnowarnlist'>true</attribute>
                        </task>
                        <task name='resgen'>
                            <attribute name='exename'>resgen</attribute>
                            <attribute name='supportsassemblyreferences'>true</attribute>
                        </task>
                        <task name='al'>
                            <attribute name='exename'>al</attribute>
                        </task>
                        <task name='delay-sign'>
                            <attribute name='exename'>sn</attribute>
                        </task>
                        <task name='license'>
                            <attribute name='exename'>lc</attribute>
                            <attribute name='supportsassemblyreferences'>true</attribute>
                        </task>
                        <task name='ilasm'>
                            <attribute name='exename'>ilasm</attribute>
                        </task>
                        <task name='ildasm'>
                            <attribute name='exename'>ildasm</attribute>
                        </task>
                    </tasks>
                </framework>
            </platform>
        </frameworks>
        <properties>
            <!-- properties defined here are accessible to all build files -->
            <!-- <property name='foo' value = 'bar' readonly='false' /> -->
        </properties>
    </nant>
    <!--
        This section contains the log4net configuration settings.

        By default, no messages will be logged to the log4net logging infrastructure.

        To enable the internal logging, set the threshold attribute on the log4net element
        to 'ALL'.

        When internal logging is enabled, internal messages will be written to the 
        console.
    -->
    <log4net threshold='OFF'>
        <appender name='ConsoleAppender' type='log4net.Appender.ConsoleAppender'>
            <layout type='log4net.Layout.PatternLayout'>
                <param name='ConversionPattern' value='[%c{2}:%m  - [%x] &lt;%X{auth}&gt;]%n' />
            </layout>
        </appender>
        <appender name='RollingLogFileAppender' type='log4net.Appender.RollingFileAppender'>
            <param name='File' value='${APPDATA}\\NAnt\\NAnt.log' />
            <param name='AppendToFile' value='true' />
            <param name='MaxSizeRollBackups' value='2' />
            <param name='MaximumFileSize' value='500KB' />
            <param name='RollingStyle' value='Size' />
            <param name='StaticLogFileName' value='true' />
            <layout type='log4net.Layout.PatternLayout'>
                <param name='ConversionPattern' value='[%c{2}:%m  - [%x] &lt;%X{auth}&gt;]%n' />
            </layout>
        </appender>
        <!-- Setup the root category, add the appenders and set the default level -->
        <root>
            <!-- Only log messages with severity ERROR (or higher) -->
            <level value='ERROR' />
            <!-- Log messages to the console -->
            <appender-ref ref='ConsoleAppender' />
            <!-- Uncomment the next line to enable logging messages to the NAnt.log file -->
            <!-- <appender-ref ref='RollingLogFileAppender' /> -->
        </root>
        <!-- Specify the priority for some specific categories -->
        <!--
        <logger name='NAnt.Core.TaskBuilderCollection'>
            <level value='DEBUG' />
        </logger>
        <logger name='NAnt'>
            <level value='INFO' />
        </logger>
        -->
    </log4net>
    <runtime>
        <assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>
            <probing privatePath='lib' />
        </assemblyBinding>
    </runtime>
    <startup>
        <supportedRuntime version='v2.0.40607' />
        <supportedRuntime version='v1.1.4322' />
        <supportedRuntime version='v1.0.3705' />
    </startup>
</configuration>");

			//return new Project(@"c:\sharpcover\SharpCover.build", Level.Info, 0, configdoc);
			return new Project(@"c:\sharpcover\SharpCover.build", Level.Info, 0);
		}
	}
}