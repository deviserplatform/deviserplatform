$winRuntime = "win-x64"
$linuxRuntime = "linux-x64"
$macOSRuntime = "osx-x64"
$distPath = "dist\"
$tempPath= $distPath+"\temp\"

function Get-Platform-Version {
	$xml = [xml](Get-Content Directory.Build.props)
	$version = $xml.Project.PropertyGroup.VersionPrefix
	if($xml.Project.PropertyGroup.VersionSuffix){
		$version += "-"+ $xml.Project.PropertyGroup.VersionSuffix
	}
	return $version
}

$version = Get-Platform-Version
Write-Output $version

$fileName = "DeviserPlatform_Install_" +$version
$outPutWin= $distPath + $fileName+"-"+$winRuntime+".zip"
$outPutLinux= $distPath + $fileName+"-"+$linuxRuntime+".zip"
$outPutMacOS= $distPath + $fileName+"-"+$macOSRuntime+".zip"


#Cleanup
Remove-Item ($distPath+"*") -Force -Recurse

#Windows Runtime
dotnet publish .\src\Deviser.WI\Deviser.WI.csproj --self-contained --output ("..\..\"+$tempPath) --runtime $winRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutWin
Remove-Item ($tempPath+"\*") -Force -Recurse

#Linux Runtime
dotnet publish .\src\Deviser.WI\Deviser.WI.csproj --self-contained --output ("..\..\"+$tempPath) --runtime $linuxRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutLinux
Remove-Item $tempPath -Force -Recurse

#macOS Runtime
dotnet publish .\src\Deviser.WI\Deviser.WI.csproj --self-contained --output ("..\..\"+$tempPath) --runtime $macOSRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutMacOS
Remove-Item $tempPath -Force -Recurse