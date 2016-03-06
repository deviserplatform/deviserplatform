using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class PageContentTranslation
    {
        public Guid Id { get; set; }
        public string ContentData { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CultureCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid PageContentId { get; set; }
        public PageContent PageContent { get; set; }
    }
}
