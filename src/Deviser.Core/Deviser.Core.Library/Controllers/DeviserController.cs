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
                var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
                var requestCulture = requestCultureFeature.RequestCulture.UICulture;
                return requestCulture;
            }
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //    AppContext returnValue = Session.GetObjectFromJson<AppContext>("AppContext");
        //    if (returnValue == null)
        //    {
        //        returnValue = new AppContext();
        //        Session.SetObjectAsJson("AppContext", returnValue);
        //    }
        //    AppContext = returnValue;
        //}

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    base.OnActionExecuted(context);
            
        //    if (AppContext == null)
        //    {
        //        AppContext = new AppContext();                
        //    }
        //    Session.SetObjectAsJson("AppContext", AppContext);
        //}
    }
}
