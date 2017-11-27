using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class LayoutTypeProperty
    {   
        public Guid PropertyId { get; set; }
        public Guid LayoutTypeId { get; set; }
        public virtual Property Property { get; set; }
        public virtual LayoutType LayoutType { get; set; }
    }
}
