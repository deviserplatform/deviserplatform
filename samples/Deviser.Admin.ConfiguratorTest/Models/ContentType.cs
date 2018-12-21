using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ContentType
    {
        public ContentType()
        {
            ContentTypeProperty = new HashSet<ContentTypeProperty>();
            PageContent = new HashSet<PageContent>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string IconClass { get; set; }
        public string IconImage { get; set; }
        public bool? IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public virtual ICollection<ContentTypeProperty> ContentTypeProperty { get; set; }
        public virtual ICollection<PageContent> PageContent { get; set; }
    }
}
