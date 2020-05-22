using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserController : Controller
    {
        private IScopeService _scopeService;

        protected bool IsAjaxRequest
        {
            get
            {
                StringValues isAjaxRequest;
                if (Request.Headers.TryGetValue("IsAjaxRequest", out isAjaxRequest))
                {
                    return bool.Parse(isAjaxRequest.ToString());
                }
                return false;

            }
        }

        protected IScopeService ScoperService
        {
            get
            {
                if (_scopeService == null)
                {
                    _scopeService = HttpContext.RequestServices.GetService<IScopeService>();
                }
                return _scopeService;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return ScoperService.PageContext.CurrentCulture;
            }
        }

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


    }
}
