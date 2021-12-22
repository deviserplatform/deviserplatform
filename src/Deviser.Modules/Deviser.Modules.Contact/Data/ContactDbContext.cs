using System.IO;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data;
using Deviser.Core.Data.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Deviser.Modules.ContactForm.Data
{
    public class ContactDbContext : ModuleDbContext
    { 
        public ContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {            
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PageModuleId).IsRequired();
                entity.Property(e => e.Data).IsRequired();
                entity.Property(e => e.CreatedOn).IsRequired();
                entity.Property(e => e.CreatedBy);
                entity.ToTable("dmc_Contact");
            });
        }

        public virtual DbSet<Contact> Contact { get; set; }
    }

    public class MySqlContactDbContext : ContactDbContext
    {
        public MySqlContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {

        }
    }

    public class PostgreSqlContactDbContext : ContactDbContext
    {
        public PostgreSqlContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {

        }
    }

    public class SqlLiteContactDbContext : ContactDbContext
    {
        public SqlLiteContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {

        }
    }

    public class SqlServerContactDbContext : ContactDbContext
    {
        public SqlServerContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {

        }
    }

    public class SqlLiteDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlLiteContactDbContext>
    {
        public SqlLiteContactDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ContactDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlLite]);
            builder.UseSqlite(connectionString, b => b.MigrationsAssembly("Deviser.Modules.ContactForm"));
            return new SqlLiteContactDbContext(builder.Options);
        }
    }

    public class SqlServerDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlServerContactDbContext>
    {
        public SqlServerContactDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ContactDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlServer]);
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Modules.ContactForm"));
            return new SqlServerContactDbContext(builder.Options);
        }
    }

    public class PostgreSqlDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlContactDbContext>
    {
        public PostgreSqlContactDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ContactDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.PostgreSql]);
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.Modules.ContactForm"));
            return new PostgreSqlContactDbContext(builder.Options);
        }
    }

    public class MySqlDbContextContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlContactDbContext>
    {
        public MySqlContactDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ContactDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.MySql]);
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("Deviser.Modules.ContactForm"));
            return new MySqlContactDbContext(builder.Options);
        }
    }
}
