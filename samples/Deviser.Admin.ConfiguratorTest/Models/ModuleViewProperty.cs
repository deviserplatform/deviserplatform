using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ModuleViewProperty
    {
        public Guid ModuleActionId { get; set; }
        public Guid PropertyId { get; set; }

        public virtual ModuleView ModuleView { get; set; }
        public virtual Property Property { get; set; }
    }
}
