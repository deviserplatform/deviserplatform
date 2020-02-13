using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class PagePermission
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Page Page { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
