@echo off
if not exist "TestResults" mkdir "TestResults"
tools\NUnit\bin\nunit-console.exe /config=SharpCover.nunit /xml=TestResults\NUnitResults-Sharpcover.xml
