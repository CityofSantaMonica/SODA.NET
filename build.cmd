@echo off

:: make sure we have nuget.exe in the path
set PATH=%PATH%;%~dp0tools\nuget

:: make sure nuget is updated
nuget update -self

:: build the CSM.SodaDotNet nuget package
cd SODA
nuget pack SODA.csproj -Prop Configuration=Release

:: build the CSM.SodaDotNet.Utilities nuget package
cd ..\Soda.Utilities
nuget pack SODA.Utilities.csproj -IncludeReferencedProjects -Prop Configuration=Release

pause