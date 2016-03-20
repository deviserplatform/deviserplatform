using Microsoft.AspNet.Identity.EntityFramework;

namespace Deviser.Core.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }       
        public string LastName { get; set; }
    }
}
