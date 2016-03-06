using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class PageContent
    {
        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public string TypeInfo { get; set; }        
        public DateTime? CreatedDate { get; set; }        
        public bool IsDeleted { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int PageId { get; set; }
        public int SortOrder { get; set; }
        public virtual Page Page { get; set; }
        public virtual ICollection<PageContentTranslation> PageContentTranslation { get; set; }
    }
}
