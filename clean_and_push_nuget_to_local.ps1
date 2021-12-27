Remove-Item -Path "$($env:USERPROFILE)\.nuget\packages\deviser*" -Force -Recurse
Remove-Item -Path "C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\deviser*" -Force -Recurse
dotnet nuget push .\release\nuget\*.nupkg -s "C:\Program Files (x86)\Microsoft SDKs\NuGetPackages"