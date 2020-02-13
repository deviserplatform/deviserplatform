using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Page
    {
        public Page()
        {
            InverseParent = new HashSet<Page>();
            PageContent = new HashSet<PageContent>();
            PageModule = new HashSet<PageModule>();
            PagePermission = new HashSet<PagePermission>();
            PageTranslation = new HashSet<PageTranslation>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsIncludedInMenu { get; set; }
        public bool IsSystem { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LayoutId { get; set; }
        public int? PageLevel { get; set; }
        public int? PageOrder { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime? StartDate { get; set; }
        public string ThemeSrc { get; set; }
        public Guid? PageTypeId { get; set; }
        public float SiteMapPriority { get; set; }

        public virtual Page Parent { get; set; }
        public virtual ICollection<Page> InverseParent { get; set; }
        public virtual ICollection<PageContent> PageContent { get; set; }
        public virtual ICollection<PageModule> PageModule { get; set; }
        public virtual ICollection<PagePermission> PagePermission { get; set; }
        public virtual ICollection<PageTranslation> PageTranslation { get; set; }
    }
}
