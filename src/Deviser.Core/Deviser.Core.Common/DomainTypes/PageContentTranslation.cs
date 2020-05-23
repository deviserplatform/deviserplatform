using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageContentTranslation
    {
        public Guid Id { get; set; }
        public string ContentData { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CultureCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid PageContentId { get; set; }
    }
}
