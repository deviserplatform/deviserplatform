using Deviser.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Deviser.Core.Common.DomainTypes;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.Blog.DTO
{
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

    public class Category : IComparable<Category>
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Name { get; set; }

        public List<Post> Posts { get; set; }

        public int PostCount { get; set; }

        public int CompareTo(Category other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
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
        [FieldInfo(FieldType.TextArea)]
        public string Summary { get; set; }

        [Order]
        [FieldInfo(FieldType.Image)]
        public string Thumbnail { get; set; }

        [Order]
        [FieldInfo(FieldType.RichText)]
        public string Content { get; set; }

        [Order]
        public string Status { get; set; }

        [Order]
        public bool IsCommentEnabled { get; set; }

        [Order]
        public Guid BlogId { get; set; }

        [Order]
        public Guid CategoryId { get; set; }

        [Order]
        public Blog Blog { get; set; }

        [Order]
        public Category Category { get; set; }

        [Order]
        public ICollection<Tag> Tags { get; set; }

        [Order]
        public List<Comments> Comments { get; set; }

        public User CreatedByUser { get; set; }

        public User ModifiedByUser { get; set; }

        [Order]
        public DateTime CreatedOn { get; set; }

        [Order]
        public Guid CreatedBy { get; set; }
        
        [Order]
        public DateTime ModifiedOn { get; set; }
        
        [Order]
        public Guid ModifiedBy { get; set; }

    }

    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
    }


    public class Comments
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public Guid PostId { get; set; }
    }
}
