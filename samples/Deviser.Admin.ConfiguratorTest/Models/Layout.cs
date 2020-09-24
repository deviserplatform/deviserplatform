using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Layout
    {
        public Guid Id { get; set; }
        public string Config { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
