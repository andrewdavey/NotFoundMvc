@echo Off
REM http://docs.myget.org/docs/reference/custom-build-scripts

ECHO echoing nuget
ECHO %nuget%

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set nuget=
if "%nuget%" == "" (
    set nuget=nuget
)

ECHO ** Package restore w/ %nuget%
cmd /c %nuget% restore "src\build.sln"

ECHO ---
ECHO ** Build / %config%
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild "src\build.sln" /t:rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=normal /nr:false

ECHO ---
ECHO ** Package pack w/ %nuget% / version: %version%
mkdir Build
cmd /c %nuget% pack "src\NotFoundMvc\NotFoundMvc.csproj" -symbols -o Build -p Configuration=%config% %version%
