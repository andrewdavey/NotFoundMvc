@echo Off
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

REM Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild "src\NotFoundMvc.sln" /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build
cmd /c %nuget% pack "src\NotFoundMvc\NotFoundMvc.csproj" -symbols -o Build -p Configuration=%config% %version%
