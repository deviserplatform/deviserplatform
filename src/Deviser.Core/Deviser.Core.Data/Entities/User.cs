using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

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
    }
}
