#Deviser Modules

To run module migration

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the module project as DefaultProject in Package Manager Console
3. Add-Migration InitalSchema -Context {ModuleDbContextName}
4. Update-Database -Context {ModuleDbContextName}

## Using Console
1. change working directory to module project
2. dotnet ef migrations add InitalSchema -c {ModuleDbContextName}
4. dotnet ef database update -c {ModuleDbContextName}