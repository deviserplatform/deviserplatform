using Deviser.Core.Data;
using Deviser.Core.Data.Installation.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Web
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeviserDbContext>
    {
        public DeviserDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new DeviserDbContext(builder.Options);
        }
    }

    public class SqlLiteDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlLiteDbContext>
    {
        public SqlLiteDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            builder.UseSqlite(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            //builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            //builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new SqlLiteDbContext(builder.Options);
        }
    }

    public class SqlServerDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new SqlServerDbContext(builder.Options);
        }
    }

    public class PostgreSqlDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlDbContext>
    {
        public PostgreSqlDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new PostgreSqlDbContext(builder.Options);
        }
    }

    public class MySqlDbContextContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public MySqlDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseMySql(connectionString, b => b.MigrationsAssembly("Deviser.Core.Data"));
            return new MySqlDbContext(builder.Options);
        }
    }
}
