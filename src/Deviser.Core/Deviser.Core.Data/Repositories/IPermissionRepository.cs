using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IPermissionRepository
    {
        IList<Permission> GetPagePermissions();
        IList<Permission> GerPermissions();
    }
}