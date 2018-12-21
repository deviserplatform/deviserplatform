using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class LayoutTypeProperty
    {
        public Guid LayoutTypeId { get; set; }
        public Guid PropertyId { get; set; }

        public virtual LayoutType LayoutType { get; set; }
        public virtual Property Property { get; set; }
    }
}
