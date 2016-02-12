# Change Log

	Version 1.0 RC1:
		Initial Visual Basic.NET support.
		A Movers and Shakers area of the report has been created detailing which things have changed that
		have affected the coverage percentage.
		NCover doesn't require a dummy file to be included in your solution any more.

	Version 0.9:
		Report is now generated as xml and then transformed by xslt to html.
		NCover now checks all code paths including inside the innermost nested 'if'
		(which it didn't before) and also that every catch block is taken.
		The only things NCover doesn't check are simple setters, ?s and select statements.

		Command line now available. See example 'ncover.cmd'
		DevEnv example build file is available 'Example-devenv.build'.
		Example build file for a two report project is available in 'Example.build'.
		BugFix: Devenv builds had their dummy files deleted. Now fixed.
	Version 0.8:
		Magic Build listener axed in favour of using the 'nrecover' task to deinstrument when
		the build file sees fit. (It shouldn't affect non-instrumented code. Thanks Manfred).
		Bug fixed: last coverage point could never be covered due to >= instead of >.
		Improved string literal handling.
	Version 0.7:
		Last version (0.6.x) had an obvious flaw:- if the build machine doen't build it would terminate
		the build leaving it in an instrumented state. NCover is now a build listener so will
		now uninstrument the code whether the build succeeds or fails.

		In case anyone ctrl-c's the build or kills the process, a new task 'nrecover' has been
		created. This should be safe to run on instrumented or non-instrumented code and will
		ensure that the code is definitely uninstrumented. For a fool proof build process, it is
		recommended that the nrecover task is run prior to CVS update in the build machine.

	Version 0.6:
		Version 0.6 is the first version we belive will be mature enough to withstand being 
		in the middle of continuous build processes. 
		Coverage history is provided if you add the attribute history="filename" to the 
		ncoverreport task.
		Upgraded solution and projects to V.S. 2003.
		If there is a dummy CoverageCounter file (for devenv builds) this is now moved and 
		put back once the report is written.

	Version 0.5:
		Project directory restructuring to provide more rigour.

	Version 0.4: 

		Removes instrumentation due to cruise control picking up modifications and leading to 
		merge conflicts on build machine. Not good.

	Version 0.3:

		Version of ncover displayed on output report.

	Version 0.2:

		Handles reinstrumenting source code better.
		NCoverCheck.cs excluded by default.
		CSCover.CSCover is now public and can have its name and namespace defined.
		OutputDir renamed to PublishDir

	Version 0.1:

		Initial nant version.
