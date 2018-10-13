#Deviser Modules

To run module migration

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the module project
3. Add-Migration InitalSchema -Context {ModuleDbContextName}

## Using Console
1. chagne working directory to module project
2. dotnet ef migrations add InitalSchema -c {ModuleDbContextName}