$winRuntime = "win-x64"
$linuxRuntime = "linux-x64"
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

Remove-Item ($distPath+"*") -Force -Recurse

dotnet publish .\src\Deviser.WI\Deviser.WI.csproj --self-contained --output ("..\..\"+$tempPath) --runtime $winRuntime

Compress-Archive -Path ($tempPath+"\*") $outPutWin

Remove-Item ($tempPath+"\*") -Force -Recurse

dotnet publish .\src\Deviser.WI\Deviser.WI.csproj --self-contained --output ("..\..\"+$tempPath) --runtime $linuxRuntime

Compress-Archive -Path ($tempPath+"\*") $outPutLinux

Remove-Item $tempPath -Force -Recurse