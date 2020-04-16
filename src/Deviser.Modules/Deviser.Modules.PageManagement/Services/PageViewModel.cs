using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Modules.PageManagement.Services
{
    public class PageViewModel : Page
    {
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
