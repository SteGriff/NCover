@echo off
if not exist "TestResults" mkdir "TestResults"
tools\NUnit\bin\nunit-console.exe SharpCover.nunit /xml=TestResults\NUnitResults-SharpCover.xml
