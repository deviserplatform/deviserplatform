using Deviser.Admin.Attributes;
using Deviser.Core.Data.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.Blog.Models
{
    public class BlogDbContext : ModuleDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Name).IsRequired();
                entity.HasIndex(p => p.Name).IsUnique();
                entity.Property(p => p.CreatedOn).IsRequired();
                entity.Property(p => p.CreatedBy).IsRequired();
                entity.ToTable("dm_blog_Blog");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("dm_blog_Category");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Title).IsRequired();
                entity.Property(p => p.Slug).IsRequired();
                entity.HasIndex(p => p.Slug).IsUnique();
                entity.Property(p => p.Status).IsRequired();
                entity.Property(p => p.CreatedOn).IsRequired();

                entity.HasOne(p => p.Blog).WithMany(c => c.Posts).HasForeignKey(p => p.BlogId);
                entity.HasOne(p => p.Category).WithMany(c => c.Posts).HasForeignKey(p => p.CategoryId);
                entity.HasMany(p => p.Tags)
                    .WithMany(p => p.Posts)
                    .UsingEntity(j => j.ToTable("dm_blog_PostTag"));
                entity.ToTable("dm_blog_Post");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.HasIndex(t => t.Name).IsUnique();
                entity.ToTable("dm_blog_Tag");
            });
            
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasForeignKey(d => d.PostId).OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("dm_blog_Comment");
            });
        }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
    }

    public class Blog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        [FieldInfo(FieldType.TextArea)]
        public string Summary { get; set; }

        [FieldInfo(FieldType.Image)]
        public string Thumbnail { get; set; }

        [FieldInfo(FieldType.RichText)]
        public string Content { get; set; }
        public string Status { get; set; }
        public bool IsCommentEnabled { get; set; }
        public Guid BlogId { get; set; }
        public Guid CategoryId { get; set; }
        public Blog Blog { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public List<Comments> Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class Comments
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }


    public class MySqlBlogDbContext : BlogDbContext
    {
        public MySqlBlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }
    }

    public class PostgreSqlBlogDbContext : BlogDbContext
    {
        public PostgreSqlBlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }
    }

    public class SqlLiteBlogDbContext : BlogDbContext
    {
        public SqlLiteBlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }
    }

    public class SqlServerBlogDbContext : BlogDbContext
    {
        public SqlServerBlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }
    }
    public class SqlLiteDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlLiteBlogDbContext>
    {
        public SqlLiteBlogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlLite]);
            builder.UseSqlite(connectionString, b => b.MigrationsAssembly("Deviser.Modules.Blog"));
            return new SqlLiteBlogDbContext(builder.Options);
        }
    }

    public class SqlServerDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlServerBlogDbContext>
    {
        public SqlServerBlogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.SqlServer]);
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.Modules.Blog"));
            return new SqlServerBlogDbContext(builder.Options);
        }
    }

    public class PostgreSqlDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlBlogDbContext>
    {
        public PostgreSqlBlogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.PostgreSql]);
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.Modules.Blog"));
            return new PostgreSqlBlogDbContext(builder.Options);
        }
    }

    public class MySqlDbContextContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlBlogDbContext>
    {
        public MySqlBlogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            var connectionString = configuration.GetConnectionString(Globals.ConnectionStringKeys[DatabaseProvider.MySql]);
            builder.UseMySql(connectionString, b => b.MigrationsAssembly("Deviser.Modules.Blog"));
            return new MySqlBlogDbContext(builder.Options);
        }
    }
}
