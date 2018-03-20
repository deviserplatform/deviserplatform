using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ModuleProperty
    {          
            public Guid PropertyId { get; set; }
            public Guid ModuleId { get; set; }
            public virtual Property Property { get; set; }
            public virtual Module Module { get; set; }
       
    }
}
