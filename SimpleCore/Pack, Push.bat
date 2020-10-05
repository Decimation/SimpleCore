@echo off

del *.nupkg

dotnet pack -c Release -o %cd%


dotnet nuget push "*.nupkg"

pause