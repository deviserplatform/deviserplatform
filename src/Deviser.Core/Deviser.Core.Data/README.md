#Deviser Modules

Goal of Deviser Platform is to support multiple database providers. Multiple database providers are supported using multiple context types as explained in [Microsoft docs](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli#using-multiple-context-types)

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the Deviser.Core.Data project as DefaultProject in Package Manager Console
3. Add-Migration InitalSchema -Context SqlLiteDbContext -OutputDir Migrations\SqlLite
4. Add-Migration InitalSchema -Context SqlServerDbContext -OutputDir Migrations\SqlServer
5. Add-Migration InitalSchema -Context PostgreSqlDbContext -OutputDir Migrations\Postgre
6. Add-Migration InitalSchema -Context MySqlDbContext -OutputDir Migrations\MySql
7. Update-Database -Context {Provider}DbContext (Only on Deviser Platform development mode)

## Using Console
1. change working directory to Deviser.Core.Data project
2. dotnet ef migrations add InitalSchema --context SqlLiteDbContext --output-dir Migrations\SqlLite
3. dotnet ef migrations add InitalSchema --context SqlServerDbContext --output-dir Migrations\SqlServer
4. dotnet ef migrations add InitalSchema --context PostgreSqlDbContext --output-dir Migrations\Postgre
5. dotnet ef migrations add InitalSchema --context MySqlDbContext --output-dir Migrations\MySql
4. dotnet ef database update -context {Provider}DbContext (Only on Deviser Platform development mode)

## Note
- Make sure nuget package Microsoft.EntityFrameworkCore.Tools is installed on startup project, if not `Install-Package Microsoft.EntityFrameworkCore.Tools`
- Make sure that IDesignTimeDbContextFactory implementation is in startup project