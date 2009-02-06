@echo off
SharpCover-Console\bin\Debug\SharpCover-Console.exe /basedir:. /recurse:"ExampleCSharpLibrary\*.cs|ExampleCSharpLibrary\*.csproj" /reportname:ExampleCSharp /publishdir:PublishDir
