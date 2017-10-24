using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public User(string userName) : this()
        {
            UserName = userName;
        }

        public string FirstName { get; set; }       
        public string LastName { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole> ();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; } = new List<IdentityUserClaim<Guid>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<Guid>> UserLogins { get; } = new List<IdentityUserLogin<Guid>>();
    }
}
