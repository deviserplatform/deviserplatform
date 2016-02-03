using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.DomainTypes
{
    public class ModuleContext
    {
        public Module ModuleInfo { get; set; }
        public Guid PageModuleId { get; set; }
    }
}
