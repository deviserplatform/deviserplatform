using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class PageContent
    {
        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public string Properties { get; set; }
        public int SortOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid PageId { get; set; }
        public Guid ContentTypeId { get; set; }
        public virtual Deviser.Core.Data.Entities.Page Page { get; set; }
        public virtual ContentType ContentType { get; set; }
        public virtual ICollection<Deviser.Core.Data.Entities.PageContentTranslation> PageContentTranslation { get; set; }
    }
}
