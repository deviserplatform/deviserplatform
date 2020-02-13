using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class User
    {
        public User()
        {
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            UserClaim = new HashSet<UserClaim>();
            UserLogin = new HashSet<UserLogin>();
            UserRole = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<UserClaim> UserClaim { get; set; }
        public virtual ICollection<UserLogin> UserLogin { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
