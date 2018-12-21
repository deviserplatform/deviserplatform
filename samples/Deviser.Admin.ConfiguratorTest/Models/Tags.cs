using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Tags
    {
        public Tags()
        {
            PostTags = new HashSet<PostTags>();
        }

        public Guid TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<PostTags> PostTags { get; set; }
    }
}
