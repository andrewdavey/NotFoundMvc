mkdir input\lib\net40
del /Q input\lib\net40\*.*

msbuild ..\src\NotFoundMvc\NotFoundMvc.csproj /p:Configuration=Release;OutputPath=..\..\package\input\lib\net40

mkdir output
..\tools\nuget.exe pack /o output input\notfoundmvc.nuspec