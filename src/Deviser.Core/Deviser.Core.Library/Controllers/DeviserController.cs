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
                if(Session.GetObjectFromJson<AppContext>("AppContext") == null)
                {
                    Session.SetObjectAsJson("AppContext", new AppContext());
                }
                return Session.GetObjectFromJson<AppContext>("AppContext");
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
