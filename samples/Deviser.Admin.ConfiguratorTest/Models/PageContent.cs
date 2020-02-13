using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class PageContent
    {
        public PageContent()
        {
            ContentPermission = new HashSet<ContentPermission>();
            PageContentTranslation = new HashSet<PageContentTranslation>();
        }

        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public Guid ContentTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? InheritEditPermissions { get; set; }
        public bool? InheritViewPermissions { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid PageId { get; set; }
        public string Properties { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }

        public virtual ContentType ContentType { get; set; }
        public virtual Page Page { get; set; }
        public virtual ICollection<ContentPermission> ContentPermission { get; set; }
        public virtual ICollection<PageContentTranslation> PageContentTranslation { get; set; }
    }
}
