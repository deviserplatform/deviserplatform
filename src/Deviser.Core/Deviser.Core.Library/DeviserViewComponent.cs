using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library
{
    public class DeviserViewComponent : ViewComponent
    {
        IScopeService scopeService;
        public DeviserViewComponent(IScopeService scopeService)
        {
            this.scopeService = scopeService;
        }

        public new ViewViewComponentResult View()
        {
            return base.View();
        }

        public new ViewViewComponentResult View(string viewName)
        {            
            return base.View(viewName);
        }

        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            return base.View<TModel>(model);
        }

        public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
        {
            return base.View<TModel>(viewName, model);
        }

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {           
            return View();
        }
    }
}
