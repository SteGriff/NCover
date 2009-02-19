@echo off
call run_UnCover_Examples.cmd
SharpCover-Console\bin\Debug\SharpCover-Console.exe /recurse:ExampleCSharpLibrary\*.cs* /reportname:ExampleCSharp /publishdir:TestResults
tools\NUnit\bin\nunit-console.exe ExampleCSharpLibraryTests\bin\Debug\ExampleCSharpLibraryTests.dll /xml=TestResults\NUnitResults-ExampleCSharpLibrary.xml
SharpCoverReport\bin\Debug\SharpCoverReport.exe /reportname:ExampleCSharp /publishdir:TestResults
