#Deviser Modules

Goal of Deviser Platform is to support multiple database providers. Multiple database providers are supported using multiple context types as explained in [Microsoft docs](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli#using-multiple-context-types)

## Using Visual Sutdio
1. Open Package Manager Console
2. Select the module project as DefaultProject in Package Manager Console
3. Add-Migration InitalSchema -Context MySqlContactDbContext -OutputDir Migrations\MySql
4. Add-Migration InitalSchema -Context PostgreSqlContactDbContext -OutputDir Migrations\Postgre
5. Add-Migration InitalSchema -Context SqlLiteContactDbContext -OutputDir Migrations\SqlLite
6. Add-Migration InitalSchema -Context SqlServerContactDbContext -OutputDir Migrations\SqlServer
7. Update-Database -Context {Provider}DbContext (Only on Deviser Platform development mode)

## Using Console
1. change working directory to module project
2.dotnet ef migrations add InitalSchema --context SqlLiteContactDbContext --output-dir Migrations\SqlLite
3.dotnet ef migrations add InitalSchema --context SqlServerContactDbContext --output-dir Migrations\SqlServer
4.dotnet ef migrations add InitalSchema --context PostgreSqlContactDbContext --output-dir Migrations\Postgre
5.dotnet ef migrations add InitalSchema --context MySqlContactDbContext --output-dir Migrations\MySql
4. dotnet ef database update -context {Provider}DbContext (Only on Deviser Platform development mode)