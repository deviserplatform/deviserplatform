 #!/bin/bash
dotnet msbuild build.targets -property:Configuration=Release;VersionSuffix=$buildNumber
