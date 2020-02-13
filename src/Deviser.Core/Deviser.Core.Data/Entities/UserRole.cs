using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Core.Data.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
