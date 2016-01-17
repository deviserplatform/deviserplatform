using Deviser.Core.Library.Extensions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class DeviserViewComponent : ViewComponent
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
                if (Session.GetObjectFromJson<AppContext>("AppContext") == null)
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

        public new ViewViewComponentResult View()
        {
            AddAppContext();
            return base.View();
        }

        public new ViewViewComponentResult View(string viewName)
        {
            AddAppContext();
            return base.View(viewName);
        }

        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            AddAppContext();
            return base.View<TModel>(model);
        }

        public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
        {
            AddAppContext();
            return base.View<TModel>(viewName, model);
        }

        private void AddAppContext()
        {
            if (AppContext != null)
                ViewBag.AppContet = AppContext;
            //else
                //TODO:Redirect to home page
        }
    }
}
