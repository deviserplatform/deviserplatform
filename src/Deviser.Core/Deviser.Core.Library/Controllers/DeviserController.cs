using Deviser.Core.Library.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Deviser.Core.Library.Services;
using Microsoft.Extensions.DependencyInjection;

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
