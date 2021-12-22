using System.IO;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Installation.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Deviser.Core.Data.Entities
{
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeviserDbContext>
    //{
    //    public DeviserDbContext CreateDbContext(string[] args)
    //    {
    //        //Debugger.Launch();

    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.Development.json")
    //            .Build();
    //        var builder = new DbContextOptionsBuilder<DeviserDbContext>();
    //        var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.]);
    //        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
    //        return new DeviserDbContext(builder.Options);
    //    }
    //}

    public class SqlLiteDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlLiteDbContext>
    {
        public SqlLiteDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlLite]);
            builder.UseSqlite(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new SqlLiteDbContext(builder.Options);
        }
    }

    public class SqlServerDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlServer]);
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new SqlServerDbContext(builder.Options);
        }
    }

    public class PostgreSqlDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlDbContext>
    {
        public PostgreSqlDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.PostgreSql]);
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new PostgreSqlDbContext(builder.Options);
        }
    }

    public class MySqlDbContextContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public MySqlDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.MySql]);
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new MySqlDbContext(builder.Options);
        }
    }
}
