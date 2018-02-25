using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deviser.Core.Common.Extensions;

namespace Deviser.Core.Library.Middleware
{
    /// <summary>
    /// It initialize the PageContext for every non /api requests
    /// </summary>
    public class PageContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IRouter _router;
        
        public PageContextMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory,
            IRouter router)
        {
            _next = next;
            _router = router;
            _logger = loggerFactory.CreateLogger<PageContextMiddleware>();


        }

        public async Task Invoke(HttpContext httpContext,
            ILifetimeScope container)
        {

            IPageManager pageManager;
            IModuleRepository moduleRepository;
            ISettingManager settingManager;
            PageContext pageContext;
            ILanguageRepository languageRepository;
            IInstallationProvider installationManager;

            pageContext = new PageContext();

            installationManager = container.Resolve<IInstallationProvider>();

            bool isInstalled = installationManager.IsPlatformInstalled;

            if (isInstalled)
            {
                RouteContext routeContext = new RouteContext(httpContext);
                routeContext.RouteData.Routers.Add(_router);
                await _router.RouteAsync(routeContext);

                if (!httpContext.Request.Path.Value.Contains("/api") && routeContext.RouteData.Values.Values.Count > 0)
                {
                    try
                    {
                        settingManager = container.Resolve<ISettingManager>();
                        pageManager = container.Resolve<IPageManager>();
                        moduleRepository = container.Resolve<IModuleRepository>(); ;
                        languageRepository = container.Resolve<ILanguageRepository>();
                        //deviserDbContext = container.Resolve<DeviserDbContext>();

                        pageContext.IsMultilingual = languageRepository.IsMultilingual();
                        pageContext.SiteSettingInfo = settingManager.GetSiteSetting();

                        var currentCulture = GetCurrentCulture(routeContext, httpContext, pageContext.IsMultilingual);

                        pageContext.CurrentCulture = currentCulture;

                        Guid homePageId = pageContext.SiteSettingInfo.HomePageId;

                        if (homePageId != Guid.Empty)
                        {
                            pageContext.HomePage = pageManager.GetPage(homePageId);
                        }


                        //This method initilizes the PageContext based on the RouteData or Query "permalink"
                        //permalink in the url has first preference
                        string permalink = getPermalink(routeContext, httpContext);
                        if (string.IsNullOrEmpty(permalink))
                        {
                            permalink = pageContext.HomePageUrl;
                        }

                        Page currentPage = null;

                        if (string.IsNullOrEmpty(permalink))
                        {
                            currentPage = pageContext.HomePage;
                        }
                        else
                        {
                            string requestCulture = pageContext.CurrentCulture.ToString().ToLower();

                            if (permalink.Contains(requestCulture) && !pageContext.IsMultilingual)
                                permalink = permalink.Replace(requestCulture + "/", "");

                            currentPage = pageManager.GetPageByUrl(permalink, pageContext.CurrentCulture.ToString());
                        }

                        pageContext.CurrentPageId = currentPage.Id;
                        pageContext.CurrentUrl = permalink;
                        pageContext.CurrentPage = currentPage;
                        pageContext.HasPageViewPermission = pageManager.HasViewPermission(currentPage);
                        pageContext.HasPageEditPermission = pageManager.HasEditPermission(currentPage);


                        if (!httpContext.Items.ContainsKey("PageContext"))
                        {
                            httpContext.Items.Add("PageContext", pageContext);
                        }
                        else
                        {
                            httpContext.Items["PageContext"] = pageContext;
                        }

                        object moduleName;
                        object pageModuleId;
                        var moduleContext = new ModuleContext();

                        //scopeService.ModuleContext = moduleContext;


                        if (routeContext.RouteData.Values.TryGetValue("area", out moduleName))
                        {
                            moduleContext.ModuleInfo = moduleRepository.Get((string)moduleName);
                        }

                        if (routeContext.RouteData.Values.TryGetValue("pageModuleId", out pageModuleId))
                        {
                            moduleContext.PageModuleId = Guid.Parse((string)pageModuleId);

                            if (moduleContext.ModuleInfo == null)
                            {
                                moduleContext.ModuleInfo = moduleRepository.GetModuleByPageModuleId(moduleContext.PageModuleId);
                            }
                        }

                        if (moduleContext.ModuleInfo != null || moduleContext.PageModuleId != null)
                        {
                            if (!httpContext.Items.ContainsKey("ModuleContext"))
                            {
                                httpContext.Items.Add("ModuleContext", moduleContext);
                            }
                            else
                            {
                                httpContext.Items["ModuleContext"] = moduleContext;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = string.Format("Error occured while initializing site ");
                        _logger.LogError(errorMessage, ex);
                    }

                    //InitPageContext();
                    //InitModuleContext();
                }
            }


            installationManager = null;

            settingManager = null;
            pageManager = null;
            moduleRepository = null;
            languageRepository = null;
            pageContext = null;

            _logger.LogInformation("Handling request: " + httpContext.Request.Path);
            await _next.Invoke(httpContext);

            //deviserDbContext.RegisterForDispose(httpContext);

            _logger.LogInformation("Finished handling request.");
        }
        
        private CultureInfo GetCurrentCulture(RouteContext routeContext, HttpContext httpContext, bool isSiteMultilingual)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo requestCulture = null;

            if (isSiteMultilingual)
            {
                string permalink = getPermalink(routeContext, httpContext);
                var match = Regex.Match(permalink, @"[a-z]{2}-[a-z]{2}", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    requestCulture = new CultureInfo(match.Value);
                }
                else
                {
                    requestCulture = requestCultureFeature.RequestCulture.UICulture;
                }
            }
            else
            {
                //TODO: Assign default language from sitesettings
                requestCulture = requestCultureFeature.RequestCulture.UICulture; //Remove it
            }

            Globals.CurrentCulture = requestCulture;
            return requestCulture;
        }

        private string getPermalink(RouteContext routeContext, HttpContext httpContext)
        {
            //permalink in the url has first preference
            string permalink = (routeContext.RouteData.Values["permalink"] != null) ? routeContext.RouteData.Values["permalink"].ToString() : "";
            if (string.IsNullOrEmpty(permalink))
            {
                //if permalink is null, check for querystring
                permalink = httpContext.Request.Query["permalink"].ToString();
            }
            return permalink;
        }
    }
}
