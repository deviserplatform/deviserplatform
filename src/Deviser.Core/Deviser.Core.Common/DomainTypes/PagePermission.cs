using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PagePermission
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }
        public Page Page { get; set; }
        public Permission Permission { get; set; }
        public Role Role { get; set; }
    }
}
