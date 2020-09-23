using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IRoleRepository
    {
        List<Role> GetRoles();
        List<Role> GetRoles(string userName);
        Role GetRole(Guid roleId);
        Role GetRoleByName(string roleName);
        Role CreateRole(Role role);
        Role UpdateRole(Role role);
        Role DeleteRole(Guid roleId);

    }
}