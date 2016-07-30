using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Entity { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }        
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<PagePermission> PagePermissions { get; set; }
        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
        public virtual ICollection<ContentPermission> ContentPermissions { get; set; }
    }
}
