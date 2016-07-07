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
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Globals.CurrentCulture = GetCurrentCulture();
            base.OnActionExecuting(context);
        }

        //protected ISession Session
        //{
        //    get
        //    {
        //        return HttpContext.Session;
        //    }
        //}

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

        protected AppContext AppContext { get; set; }

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
            ViewBag.AppContext = AppContext;
            return base.View();
        }

        [NonAction]
        public override ViewResult View(object model)
        {
            ViewBag.AppContext = AppContext;
            return base.View(model);
        }

        [NonAction]
        public override ViewResult View(string viewName)
        {
            ViewBag.AppContext = AppContext;
            return base.View(viewName);
        }

        [NonAction]
        public override ViewResult View(string viewName, object model)
        {
            ViewBag.AppContext = AppContext;
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
