using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class PageTranslation
    {
        public Guid PageId { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsLinkNewWindow { get; set; }
        public string PageHeaderTags { get; set; }
        public virtual Page Page { get; set; }
    }
}
