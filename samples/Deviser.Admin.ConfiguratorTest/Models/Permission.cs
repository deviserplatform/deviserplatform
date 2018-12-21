using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Permission
    {
        public Permission()
        {
            ContentPermission = new HashSet<ContentPermission>();
            ModulePermission = new HashSet<ModulePermission>();
            PagePermission = new HashSet<PagePermission>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public string Entity { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ContentPermission> ContentPermission { get; set; }
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        public virtual ICollection<PagePermission> PagePermission { get; set; }
    }
}
