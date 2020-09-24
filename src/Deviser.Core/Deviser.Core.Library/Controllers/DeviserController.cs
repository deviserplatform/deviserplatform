using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserController : Controller
    {
        private IScopeService _scopeService;

        protected bool IsAjaxRequest => Request.Headers.TryGetValue("IsAjaxRequest", out var isAjaxRequest) && bool.Parse(isAjaxRequest.ToString());

        protected IScopeService ScoperService => _scopeService ??= HttpContext.RequestServices.GetService<IScopeService>();

        public CultureInfo CurrentCulture => ScoperService.PageContext.CurrentCulture;

        [NonAction]
        public override ViewResult View()
        {
            return base.View();
        }

        [NonAction]
        public override ViewResult View(object model)
        {
            return base.View(model);
        }

        [NonAction]
        public override ViewResult View(string viewName)
        {
            return base.View(viewName);
        }

        [NonAction]
        public override ViewResult View(string viewName, object model)
        {
            return base.View(viewName, model);
        }





        [NonAction]
        public async Task<ViewResult> ViewAsync()
        {
            return await Task.Run(() => View());
        }

        [NonAction]
        public async Task<ViewResult> ViewAsync(object model)
        {
            return await Task.Run(() => View(model));
        }

        [NonAction]
        public async Task<ViewResult> ViewAsync(string viewName)
        {
            return await Task.Run(() => View(viewName));
        }

        [NonAction]
        public async Task<ViewResult> ViewAsync(string viewName, object model)
        {
            return await Task.Run(() => View(viewName, model));
        }


    }
}
