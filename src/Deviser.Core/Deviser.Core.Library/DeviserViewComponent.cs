using Deviser.Core.Library.Extensions;
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
using Deviser.Core.Common;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library
{
    public class DeviserViewComponent : ViewComponent
    {
        IScopeService scopeService;
        public DeviserViewComponent(IScopeService scopeService)
        {
            this.scopeService = scopeService;
        }

        //protected ISession Session
        //{
        //    get
        //    {
        //        return HttpContext.Session;
        //    }
        //}
        
        public new ViewViewComponentResult View()
        {
            return base.View();
        }

        public new ViewViewComponentResult View(string viewName)
        {            
            return base.View(viewName);
        }

        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            return base.View<TModel>(model);
        }

        public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
        {
            return base.View<TModel>(viewName, model);
        }

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {           
            return View();
        }
    }
}
