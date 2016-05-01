using Deviser.Core.Library.Extensions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Globals.CurrentCulture = GetCurrentCulture();
            base.OnActionExecuting(context);
        }

        protected ISession Session
        {
            get
            {
                return HttpContext.Session;
            }
        }

        protected AppContext AppContext
        {
            get
            {
                AppContext returnValue = Session.GetObjectFromJson<AppContext>("AppContext");
                if (returnValue == null)
                {
                    returnValue = new AppContext();
                    Session.SetObjectAsJson("AppContext", returnValue);
                }
                returnValue.CurrentCulture = CurrentCulture;
                return returnValue;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                Session.SetObjectAsJson("AppContext", value);
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return GetCurrentCulture();
            }
        }


        public override ViewResult View()
        {
            ViewBag.AppContext = AppContext;
            return base.View();
        }

        public override ViewResult View(object model)
        {
            ViewBag.AppContext = AppContext;
            return base.View(model);
        }

        public override ViewResult View(string viewName)
        {
            ViewBag.AppContext = AppContext;
            return base.View(viewName);
        }

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
