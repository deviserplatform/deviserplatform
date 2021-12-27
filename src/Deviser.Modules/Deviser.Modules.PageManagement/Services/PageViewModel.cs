using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Modules.PageManagement.Services
{
    public class PageViewModel
    {
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
        public virtual PageType PageType { get; set; }
        public virtual AdminPage AdminPage { get; set; }

        //Non DB Properties
        public bool IsCurrentPage { get; set; }
        public bool IsBreadCrumb { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PageHeaderTags { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsLinkNewWindow { get; set; }
        public string Url { get; set; }
        public Module Module { get; set; }
        public string ModelName { get; set; }
        public string RedirectLink { get; set; }
        public Theme Theme { get; set; }

        public new PageViewModel Parent { get; set; }
        public new ICollection<PageViewModel> ChildPage { get; set; }

        //public string PageName
        //{
        //    get
        //    {
        //        PageTranslation?.FirstOrDefault(pt => string.Equals(pt.Locale, System.Threading.Thread.CurrentThread.CurrentCulture. )?.Name
        //    }
        //}

    }
}
