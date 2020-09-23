$winRuntime = "win-x64"
$linuxRuntime = "linux-x64"
$macOSRuntime = "osx-x64"
$distPath = ".\release\binary"
$tempPath= $distPath+"\temp"

function Get-Platform-Version {
	$xml = [xml](Get-Content Directory.Build.props)
	$version = $xml.Project.PropertyGroup.VersionPrefix
	if($xml.Project.PropertyGroup.VersionSuffix){
		$version += "-"+ $xml.Project.PropertyGroup.VersionSuffix
	}
	return $version
}

function Clean-And-Create-Folder($Path){
	if (Test-Path $Path){
	  Remove-Item ($Path+"\*") -Force -Recurse
	}
	New-Item -Path $Path -ItemType "directory"
}

$version = Get-Platform-Version
Write-Output $version

$fileName = "DeviserPlatform_Install_" +$version
$outPutWin= $distPath +"\" + $fileName+"-"+$winRuntime+".zip"
$outPutLinux= $distPath +"\" + $fileName+"-"+$linuxRuntime+".zip"
$outPutMacOS= $distPath +"\"+ $fileName+"-"+$macOSRuntime+".zip"


#Cleanup
Clean-And-Create-Folder -Path $distPath
Clean-And-Create-Folder -Path $tempPath

#Windows Runtime
dotnet publish .\src\DeviserApp\DeviserApp.csproj --self-contained --output ($tempPath) --runtime $winRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutWin
Remove-Item ($tempPath+"\*") -Force -Recurse

#Linux Runtime
dotnet publish .\src\DeviserApp\DeviserApp.csproj --self-contained --output ($tempPath) --runtime $linuxRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutLinux
Remove-Item $tempPath -Force -Recurse

#macOS Runtime
dotnet publish .\src\DeviserApp\DeviserApp.csproj --self-contained --output ($tempPath) --runtime $macOSRuntime
Compress-Archive -Path ($tempPath+"\*") $outPutMacOS
Remove-Item $tempPath -Force -Recurse