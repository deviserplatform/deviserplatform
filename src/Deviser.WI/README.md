#Deviser WI


## DB Migration
To run module migration

### Using Visual Sutdio
1. Open Package Manager Console
2. Select the module project as DefaultProject in Package Manager Console
3. Add-Migration {MigrationName} -Context DeviserDbContext
4. Update-Database -Context DeviserDbContext

### Using Console
1. chagne working directory to module project
2. dotnet ef migrations add {MigrationName} -c DeviserDbContext
4. dotnet ef database update -c DeviserDbContext