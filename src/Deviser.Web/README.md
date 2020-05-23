#Deviser Modules

To run module migration

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the module project as DefaultProject in Package Manager Console
3. Add-Migration InitalSchema -Context DeviserDBContext
4. Update-Database -Context DeviserDBContext
## Using Console
1. change working directory to module project
2. dotnet ef migrations add InitalSchema -c DeviserDBContext
4. dotnet ef database update -c DeviserDBContext

## Note
- Make sure nuget package Microsoft.EntityFrameworkCore.Tools is installed on startup project, if not `Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.1.1`