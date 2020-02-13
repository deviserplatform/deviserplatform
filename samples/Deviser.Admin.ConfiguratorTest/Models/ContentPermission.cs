using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ContentPermission
    {
        public Guid Id { get; set; }
        public Guid PageContentId { get; set; }
        public Guid PermissionId { get; set; }
        public Guid RoleId { get; set; }

        public virtual PageContent PageContent { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
