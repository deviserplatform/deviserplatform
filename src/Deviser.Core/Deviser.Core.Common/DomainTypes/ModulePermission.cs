using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class ModulePermission
    {
        public Guid Id { get; set; }
        public Guid PageModuleId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }        
    }
}
