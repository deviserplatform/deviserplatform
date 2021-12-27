using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class ModuleContext
    {
        public Module ModuleInfo { get; }
        public Guid PageModuleId { get; }
        public PageModule PageModule { get; }
        public IDictionary<string, Property> ModuleViewProperties { get;}

        public ModuleContext(Module module, PageModule pageModule)
        {
            ModuleInfo = module;
            PageModule = pageModule;
            PageModuleId = pageModule.Id;
            ModuleViewProperties = PageModule.Properties.ToDictionary(p => p.Name, p => p);
        }

        public ModuleContext(Module module)
        {
            ModuleInfo = module;
        }
    }
}
