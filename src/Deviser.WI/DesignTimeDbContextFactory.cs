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

namespace Deviser.WI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeviserDbContext>
    {
        public DeviserDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DeviserDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            return new DeviserDbContext(builder.Options);
        }
    }
}
