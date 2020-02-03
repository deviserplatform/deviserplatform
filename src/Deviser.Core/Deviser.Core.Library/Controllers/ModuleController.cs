using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        public ModuleContext ModuleContext
        {
            get
            {
                return ScoperService.ModuleContext;
            }
        }

        public Guid PageModuleId
        {
            get
            {
                return (Guid)RouteData.Values["pageModuleId"];
            }
        }
    }
}
