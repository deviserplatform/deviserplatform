 #!/bin/bash
echo "build number: ${1}"
dotnet msbuild build.targets /p:"Configuration=Release;VersionSuffix=beta3.${1}"
