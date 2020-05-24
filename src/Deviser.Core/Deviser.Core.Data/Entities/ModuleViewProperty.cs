using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ModuleViewProperty
    {
        public Guid PropertyId { get; set; }
        public Guid ModuleViewId { get; set; }
        public virtual Property Property { get; set; }
        public virtual ModuleView ModuleView { get; set; }

    }
}
