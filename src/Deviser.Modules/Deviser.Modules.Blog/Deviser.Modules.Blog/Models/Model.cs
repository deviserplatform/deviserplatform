using Deviser.Admin.Attributes;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Data.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.Blog.Models
{
    public class BlogDbContext : ModuleDbContext
    {
        public BlogDbContext(DbContextOptions options)
            : base(options)
        {
            ModuleMetaInfo = Global.ModuleMetaInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.CreatedOn).IsRequired();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId).ValueGeneratedOnAdd();
                entity.HasAlternateKey(e => e.TagName);
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasKey(t => new { t.PostId, t.TagId });
                entity.HasOne(pt => pt.Post).WithMany(p => p.PostTags).HasForeignKey(pt => pt.PostId);
                entity.HasOne(pt => pt.Tag).WithMany(t => t.PostTags).HasForeignKey(pt => pt.TagId);
            });
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
    }

    public class Post
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Title { get; set; }

        [Order]
        [FieldInfoAttribute(FieldType.RichText)]
        public string Content { get; set; }

        [Order]
        public List<PostTag> PostTags { get; set; }

        [Order]
        public DateTime CreatedOn { get; set; }

        [Order]
        public string CreatedBy { get; set; }
        
    }

    public class Tag
    {
        public Guid TagId { get; set; }

        public string TagName { get; set; }

        public List<PostTag> PostTags { get; set; }
    }

    public class PostTag
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
