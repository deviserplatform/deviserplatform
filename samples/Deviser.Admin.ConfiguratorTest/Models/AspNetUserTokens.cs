using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class AspNetUserTokens
    {
        public Guid UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual User User { get; set; }
    }
}
