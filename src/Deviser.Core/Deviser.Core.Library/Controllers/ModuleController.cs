using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        public ModuleContext ModuleContext => ScoperService.ModuleContext;

        public Guid PageModuleId => (Guid)RouteData.Values["pageModuleId"];
    }
}
