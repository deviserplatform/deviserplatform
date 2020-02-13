using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class UserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
