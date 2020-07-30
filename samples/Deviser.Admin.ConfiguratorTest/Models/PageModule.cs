using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class PageModule
    {
        public PageModule()
        {
            ModulePermission = new HashSet<ModulePermission>();
        }

        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public bool? InheritEditPermissions { get; set; }
        public bool? InheritViewPermissions { get; set; }
        public bool IsActive { get; set; }
        public Guid ModuleActionId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageId { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Properties { get; set; }

        public virtual Module Module { get; set; }
        public virtual ModuleView ModuleView { get; set; }
        public virtual Page Page { get; set; }
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
    }
}
