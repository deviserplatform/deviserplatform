using Deviser.Admin.Attributes;
using Deviser.Core.Data.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Deviser.Admin.Config;

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
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Title).IsRequired();
                entity.Property(p => p.Slug)
                    .IsRequired();
                entity.HasIndex(p => p.Slug).IsUnique();
                entity.Property(p => p.CreatedOn).IsRequired();
                entity.HasOne(p => p.Category).WithMany(c => c.Posts).HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.HasAlternateKey(t => t.TagName);
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasKey(pt => new { pt.PostId, pt.TagId });
                entity.HasOne(pt => pt.Post).WithMany(p => p.PostTags).HasForeignKey(pt => pt.PostId);
                entity.HasOne(pt => pt.Tag).WithMany(t => t.PostTags).HasForeignKey(pt => pt.TagId);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasForeignKey(d => d.PostId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
    }

    public class Category
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Title { get; set; }

        [Order]
        public string Slug { get; set; }

        [Order]
        [FieldInfo(FieldType.RichText)]
        public string Content { get; set; }

        [Order]
        public Guid CategoryId { get; set; }

        [Order]
        public Category Category { get; set; }

        [Order]
        public List<PostTag> PostTags { get; set; }

        [Order]
        [NotMapped]
        public IEnumerable<Tag> Tags
        {
            set
            {
                var tags = value;
                if (tags != null && tags.Count() > 0)
                {
                    PostTags = new List<PostTag>();
                    foreach (var tag in tags)
                    {
                        PostTags.Add(new PostTag
                        {
                            Post = this,
                            PostId = Id,
                            TagId = tag.Id,
                            Tag = tag
                        });
                    }
                }
            }
            get => PostTags.Select(e => e.Tag);
        }

        [Order]
        public List<Comments> Comments { get; set; }

        [Order]
        public DateTime CreatedOn { get; set; }

        [Order]
        public string CreatedBy { get; set; }

    }

    public class Tag
    {
        public Guid Id { get; set; }
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
}
