using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.WI.DTO
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public List<Role> Roles { get; set; }
    }
}