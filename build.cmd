@echo off

cls

set TARGET="Default"
if not "%1" == "" (set TARGET="%1")

set BUILDMODE="Release"
if not "%2" == "" (set BUILDMODE="%2")

set SODADIR=".\SODA"
set UTILSDIR=".\Utilities"
set SODA="%SODADIR%\SODA.csproj"
set UTILS="%UTILSDIR%\Utilities.csproj"

echo Restoring NuGet dependencies

call dotnet restore --verbosity m

echo Rebuilding solution with Configuration: %BUILDMODE%

call dotnet build SODA.sln "/p:Configuration=%BUILDMODE%" "/p:Platform=Any CPU" /t:Clean,Build

echo Finished solution rebuild

if %TARGET% == "CreatePackages" (
	echo Creating NuGet packages

	call dotnet pack %SODA% --no-build --output ..\
	call dotnet pack %UTILS% --no-build --output ..\
	
	echo Finished NuGet package creation
)

exit /B %errorlevel%