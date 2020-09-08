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

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<PagePermission> PagePermissions { get; set; }
        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
        public virtual ICollection<ContentPermission> ContentPermissions { get; set; }
    }
}
