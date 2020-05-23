using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class Page
    {
        public Page()
        {
            PageContent = new HashSet<PageContent>();
            PageModule = new HashSet<PageModule>();
            PageTranslation = new HashSet<PageTranslation>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public bool IsIncludedInMenu { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LayoutId { get; set; }
        public int? PageLevel { get; set; }
        public int? PageOrder { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? PageTypeId { get; set; }
        public string ThemeSrc { get; set; }        
        public DateTime? StartDate { get; set; }
        public float SiteMapPriority { get; set; }
        public virtual ICollection<PageContent> PageContent { get; set; }
        public virtual ICollection<PageModule> PageModule { get; set; }
        public virtual ICollection<PageTranslation> PageTranslation { get; set; }
        public virtual ICollection<PagePermission> PagePermissions { get; set; }
        public virtual Layout Layout { get; set; }
        public virtual Page Parent { get; set; }
        public virtual PageType PageType { get; set; }
        public virtual ICollection<Page> ChildPage { get; set; }

        public virtual AdminPage AdminPage { get; set; }

        //Non DB Properties
        //public bool IsActive { get; set; }
        public bool IsBreadCrumb { get; set; }
    }
}
