using Deviser.Core.Common;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Middleware
{
    public class PageContextMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private IRouter router;
        private ISiteBootstrapper siteBootstrapper;
        private RouteContext routeContext;
        private HttpContext httpContext;
        private IPageManager pageManager;

        public PageContextMiddleware(RequestDelegate next, 
            ILoggerFactory loggerFactory,
            ISiteBootstrapper siteBootstrapper,
            IPageManager pageManager,
            IRouter router)
        {
            this.next = next;
            this.router = router;
            this.pageManager = pageManager;
            //this.scopeService = scopeService;
            logger = loggerFactory.CreateLogger<PageContextMiddleware>();
            siteBootstrapper.InitializeSite();
        }

        public async Task Invoke(HttpContext context)
        {
            httpContext = context;

            if (!context.Request.Path.Value.Contains("/api"))
            {
                routeContext = new RouteContext(context);
                routeContext.RouteData.Routers.Add(router);
                await router.RouteAsync(routeContext);

                InitPageContext();
            }
            

            logger.LogInformation("Handling request: " + context.Request.Path);
            await next.Invoke(context);
            logger.LogInformation("Finished handling request.");
        }

        private Page InitPageContext()
        {
            //permalink in the url has first preference
            string permalink = (routeContext.RouteData.Values["permalink"]!=null)? routeContext.RouteData.Values["permalink"].ToString():"";

            if (string.IsNullOrEmpty(permalink))
            {
                //if permalink is null, check for querystring
                permalink = httpContext.Request.Query["permalink"].ToString();
            }

            Page currentPage = null;
            var currentCulture = GetCurrentCulture();
            if (string.IsNullOrEmpty(permalink))
            {
                permalink = Globals.HomePageUrl;
                currentPage = Globals.HomePage;
            }
            else
            {
                string requestCulture = (routeContext.RouteData.Values["culture"] != null) ? routeContext.RouteData.Values["culture"].ToString() : currentCulture.ToString().ToLower();

                if (!permalink.Contains(requestCulture))
                    permalink = $"{requestCulture}/{permalink}";

                currentPage = pageManager.GetPageByUrl(permalink, currentCulture.ToString());
            }
            PageContext pageContext = new PageContext();
            pageContext.CurrentPageId = currentPage.Id;
            pageContext.CurrentUrl = permalink;
            pageContext.CurrentPage = currentPage;
            pageContext.HasPageViewPermission = pageManager.HasViewPermission(currentPage);
            pageContext.HasPageEditPermission = pageManager.HasEditPermission(currentPage);
            pageContext.CurrentCulture = currentCulture;
            httpContext.Items.Add("PageContext", pageContext);
            //scopeService.PageContext = pageContext; //Very important!!!
            //scopeService.PageContext.CurrentCulture = currentCulture; //Very important!!!
            return currentPage;
        }

        private CultureInfo GetCurrentCulture()
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo requestCulture = null;
            string cultureKey = "culture";
            if (routeContext.RouteData.Values.ContainsKey(cultureKey) && !string.IsNullOrEmpty(routeContext.RouteData.Values[cultureKey].ToString()))
            {
                requestCulture = new CultureInfo(routeContext.RouteData.Values[cultureKey].ToString());
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
