using Deviser.Core.Library.DomainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Autofac;
using Deviser.Core.Data.DataProviders;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        private IPageProvider pageProvider;
        private IModuleProvider moduleProvider;

        //public ILifetimeScope container { get; set; }

        public ModuleController(ILifetimeScope container)
        {
            pageProvider = container.Resolve<IPageProvider>();
            moduleProvider = container.Resolve<IModuleProvider>();
        }

        public ModuleContext ModuleContext { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            object moduleName;
            object pageModuleId;

            if (context.RouteData.Values.TryGetValue("area", out moduleName))
            {
                ModuleContext = new ModuleContext();
                ModuleContext.ModuleInfo = moduleProvider.Get((string)moduleName);
            }

            if (context.RouteData.Values.TryGetValue("pageModuleId", out pageModuleId))
            {
                ModuleContext.PageModuleId = (Guid)pageModuleId;
            }
        }
    }
}
