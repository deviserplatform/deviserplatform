using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ModuleActionProperty
    {
        public Guid PropertyId { get; set; }
        public Guid ModuleActionId { get; set; }
        public virtual Property Property { get; set; }
        public virtual ModuleAction ModuleAction { get; set; }

    }
}
