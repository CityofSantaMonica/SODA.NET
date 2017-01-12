@echo off

cls

set TARGET="Default"
if not "%1" == "" (set TARGET="%1")

set BUILDMODE="Release"
if not "%2" == "" (set BUILDMODE="%2")

set MSBUILDDIR="%WINDIR%\Microsoft.NET\Framework\v4.0.30319"
set SODADIR=".\SODA"
set UTILSDIR=".\SODA.Utilities"

echo Restoring NuGet package dependencies

call nuget restore

echo Rebuilding solution with Configuration: %BUILDMODE%

call %MSBUILDDIR%\msbuild.exe SODA.sln /m "/p:Configuration=%BUILDMODE%" "/p:Platform=Any CPU" /t:Clean,Build	

echo Finished solution rebuild

if %TARGET% == "CreatePackages" (
	echo Creating NuGet packages with Configuration: %BUILDMODE%

	call xcopy %SODADIR%\bin\%BUILDMODE%\*.* %SODADIR%\lib\ /y
	call xcopy %UTILSDIR%\bin\%BUILDMODE%\*.* %UTILSDIR%\lib\ /y

	call nuget pack %SODADIR%\SODA.csproj -Properties "Configuration=%BUILDMODE%;Platform=AnyCPU"
	call nuget pack %UTILSDIR%\SODA.Utilities.csproj -IncludeReferencedProjects -Properties "Configuration=%BUILDMODE%;Platform=AnyCPU"
	
	rd /S /Q %SODADIR%\lib
	rd /S /Q %UTILSDIR%\lib

	echo Finished NuGet package creation
)

exit /B %errorlevel%