using Deviser.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Deviser.Admin.Config;

namespace Deviser.Modules.Blog.DTO
{
    public class Category : IComparable<Category>
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Name { get; set; }

        public List<Post> Posts { get; set; }

        public int CompareTo(Category other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        //public override string ToString()
        //{
        //    return Name;
        //}

        //public override string ToString()
        //{
        //    return this.Name;
        //}

        //public override bool Equals(object? obj)
        //{
        //    if (obj == null)
        //    {
        //        return false;
        //    }

        //    if (obj is Category category)
        //    {
        //        return category.Name?.Equals(this.Name, StringComparison.Ordinal) ?? false;
        //    }

        //    return obj.Equals(this);
        //}
    }

    public class Post
    {
        [Order]
        public Guid Id { get; set; }

        [Order]
        public string Title { get; set; }

        [Order]
        [FieldInfo(FieldType.RichText)]
        public string Content { get; set; }

        [Order]
        public Guid CategoryId { get; set; }

        [Order]
        public Category Category { get; set; }

        [Order]
        public ICollection<Tag> Tags { get; set; }

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
