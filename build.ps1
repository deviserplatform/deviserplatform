
$buildNumber=$args[0]
write-host "build number: $buildNumber"
dotnet msbuild build.targets /p:"Configuration=Release;VersionSuffix=beta2.$buildNumber"