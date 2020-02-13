using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Posts
    {
        public Posts()
        {
            PostTags = new HashSet<PostTags>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<PostTags> PostTags { get; set; }
    }
}
