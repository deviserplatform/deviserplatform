﻿using Deviser.Core.Library.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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
                AppContext returnValue = Session.GetObjectFromJson<AppContext>("AppContext");
                if (returnValue == null)
                {
                    returnValue = new AppContext();
                    Session.SetObjectAsJson("AppContext", returnValue);
                }
                returnValue.CurrentCulture = CurrentCulture;
                return returnValue;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
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

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {           
            return View();
        }
    }
}
