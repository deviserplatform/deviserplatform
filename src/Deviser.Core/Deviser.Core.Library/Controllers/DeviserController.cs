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

namespace Deviser.Core.Library.Controllers
{
    public class DeviserController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Globals.CurrentCulture = GetCurrentCulture();
            //string permalink = "";
            //permalink = RouteData.Values["permalink"] as string;
            //if (string.IsNullOrEmpty(permalink))
            //    permalink = context.HttpContext.Request.Query["permalink"];

            //if (!string.IsNullOrEmpty(permalink))
            //{

            //}

            base.OnActionExecuting(context);
        }

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

        public CultureInfo CurrentCulture
        {
            get
            {
                return GetCurrentCulture();
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

        private CultureInfo GetCurrentCulture()
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo requestCulture = null;
            string cultureKey = "culture";
            if (RouteData.Values.ContainsKey(cultureKey) && !string.IsNullOrEmpty(RouteData.Values[cultureKey].ToString()))
            {
                requestCulture = new CultureInfo(RouteData.Values[cultureKey].ToString());
            }
            else
            {
                requestCulture = requestCultureFeature.RequestCulture.UICulture;
            }

            Globals.CurrentCulture = requestCulture;
            return requestCulture;
        }


    }
}
