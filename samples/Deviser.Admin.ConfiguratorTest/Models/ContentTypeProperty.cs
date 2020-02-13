using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ContentTypeProperty
    {
        public Guid ContentTypeId { get; set; }
        public Guid PropertyId { get; set; }

        public virtual ContentType ContentType { get; set; }
        public virtual Property Property { get; set; }
    }
}
