using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class PageContentTranslation
    {
        public Guid Id { get; set; }
        public string ContentData { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CultureCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid PageContentId { get; set; }

        public virtual PageContent PageContent { get; set; }
    }
}
