using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ModuleActionProperty
    {
        public Guid ModuleActionId { get; set; }
        public Guid PropertyId { get; set; }

        public virtual ModuleAction ModuleAction { get; set; }
        public virtual Property Property { get; set; }
    }
}
