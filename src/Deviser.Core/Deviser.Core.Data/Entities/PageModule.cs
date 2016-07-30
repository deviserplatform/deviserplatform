using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class PageModule
    {
        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ModuleActionId { get; set; }
        public int SortOrder { get; set; }
        public Guid PageId { get; set; }
        public ModuleAction ModuleAction { get;set;}
        public virtual Module Module { get; set; }
        public virtual Page Page { get; set; }

        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
    }
}
