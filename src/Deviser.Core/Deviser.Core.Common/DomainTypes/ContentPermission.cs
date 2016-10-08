using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentPermission
    {
        public Guid Id { get; set; }
        public Guid PageContentId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }        
    }
}
