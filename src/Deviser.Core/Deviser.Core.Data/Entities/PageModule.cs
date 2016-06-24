using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class PageModule
    {
        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public bool IsDeleted { get; set; }
        public int ModuleId { get; set; }
        public int ModuleActionId { get; set; }
        public int SortOrder { get; set; }
        public int PageId { get; set; }
        public virtual Module Module { get; set; }
        public virtual Page Page { get; set; }
    }
}
