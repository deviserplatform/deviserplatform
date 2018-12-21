using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Role
    {
        public Role()
        {
            ContentPermission = new HashSet<ContentPermission>();
            ModulePermission = new HashSet<ModulePermission>();
            PagePermission = new HashSet<PagePermission>();
            RoleClaim = new HashSet<RoleClaim>();
            UserRole = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual ICollection<ContentPermission> ContentPermission { get; set; }
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        public virtual ICollection<PagePermission> PagePermission { get; set; }
        public virtual ICollection<RoleClaim> RoleClaim { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
