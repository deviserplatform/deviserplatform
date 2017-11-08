using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }

        public Role(string roleName) : this()
        {
            Name = roleName;
        }

        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; } = new List<IdentityUserRole<Guid>>();
        public virtual ICollection<PagePermission> PagePermissions { get; set; }
        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
        public virtual ICollection<ContentPermission> ContentPermissions { get; set; }
    }
}
