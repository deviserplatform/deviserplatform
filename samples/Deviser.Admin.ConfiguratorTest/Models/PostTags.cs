using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class PostTags
    {
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Tags Tag { get; set; }
    }
}
