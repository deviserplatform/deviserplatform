#Deviser Modules

To run module migration

## Run database specific migrations 
1. Add-Migration InitalSchema -Context SqlLiteDbContext -OutputDir Migrations\SqlLite
2. Add-Migration InitalSchema -Context SqlServerDbContext -OutputDir Migrations\SqlServer
3. Add-Migration InitalSchema -Context PostgreSqlDbContext -OutputDir Migrations\Postgre
4. Add-Migration InitalSchema -Context MySqlDbContext -OutputDir Migrations\MySql
5. Add-Migration InitalSchema -Context DeviserDbContext

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the Deviser.Core.Data project as DefaultProject in Package Manager Console
3. Add-Migration InitalSchema -Context DeviserDbContext
4. Update-Database -Context DeviserDbContext
## Using Console
1. change working directory to Deviser.Core.Data project
2. dotnet ef migrations add InitalSchema -c DeviserDbContext
4. dotnet ef database update -c DeviserDbContext

## Note
- Make sure nuget package Microsoft.EntityFrameworkCore.Tools is installed on startup project, if not `Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.1.1`
- Make sure that IDesignTimeDbContextFactory implementation is in startup project