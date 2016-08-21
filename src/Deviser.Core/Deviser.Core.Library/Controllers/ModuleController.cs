using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleController : DeviserController
    {
        private IPageProvider pageProvider;
        private IModuleProvider moduleProvider;
        protected IScopeService scopeService;

        //public ILifetimeScope container { get; set; }

        public ModuleController(ILifetimeScope container)
        {
            pageProvider = container.Resolve<IPageProvider>();
            moduleProvider = container.Resolve<IModuleProvider>();
            scopeService = container.Resolve<IScopeService>();
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

            if (ModuleContext == null &&
                (scopeService.ModuleContext.ModuleInfo != null || scopeService.ModuleContext.PageModuleId != null))
            {
                ModuleContext = scopeService.ModuleContext;
            }
        }
    }
}
