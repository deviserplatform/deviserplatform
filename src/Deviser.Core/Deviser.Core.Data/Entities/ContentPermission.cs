using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class ContentPermission
    {
        public Guid Id { get; set; }
        public Guid PageContentId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }
        public PageContent PageContent { get; set; }
        public Permission Permission { get; set; }
        public Role Role { get; set; }
    }
}
