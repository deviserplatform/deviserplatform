using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Middleware
{
    /// <summary>
    /// It initialize the PageContext for every non /api requests
    /// </summary>
    public class PageContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        //private readonly IRouter _router;

        public PageContextMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory /*,IRouter router*/)
        {
            _next = next;
            //_router = router;
            _logger = loggerFactory.CreateLogger<PageContextMiddleware>();


        }

        public async Task Invoke(HttpContext httpContext,
            IInstallationProvider installationManager,
            IPageManager pageManager,
            IModuleRepository moduleRepository,
            ISettingManager settingManager,
            ILanguageRepository languageRepository)
        {


            PageContext pageContext;
            pageContext = new PageContext();
            bool isInstalled = installationManager.IsPlatformInstalled;

            if (isInstalled)
            {
                //RouteContext routeContext = new RouteContext(httpContext);
                var routeData = httpContext.GetRouteData();
                //routeContext.RouteData.Routers.Add(_router);
                //await _router.RouteAsync(routeContext);

                if (!httpContext.Request.Path.Value.Contains("/api") && routeData.Values.Values.Count > 0)
                {
                    try
                    {
                        //deviserDbContext = container.Resolve<DeviserDbContext>();

                        pageContext.IsMultilingual = languageRepository.IsMultilingual();
                        pageContext.SiteSettingInfo = settingManager.GetSiteSetting();

                        var currentCulture = GetCurrentCulture(routeData, httpContext, pageContext.IsMultilingual);

                        pageContext.CurrentCulture = currentCulture;

                        Guid homePageId = pageContext.SiteSettingInfo.HomePageId;

                        if (homePageId != Guid.Empty)
                        {
                            pageContext.HomePage = pageManager.GetPageAndDependencies(homePageId);
                        }


                        //This method initilizes the PageContext based on the RouteData or Query "permalink"
                        //permalink in the url has first preference
                        string permalink = getPermalink(routeData, httpContext);
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

                            currentPage = pageManager.GetPageAndDependenciesByUrl(permalink, pageContext.CurrentCulture.ToString());
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


                        if (routeData.Values.TryGetValue("area", out moduleName))
                        {
                            moduleContext.ModuleInfo = moduleRepository.GetModule((string)moduleName);
                        }

                        if (routeData.Values.TryGetValue("pageModuleId", out pageModuleId))
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

        private CultureInfo GetCurrentCulture(RouteData routeData, HttpContext httpContext, bool isSiteMultilingual)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo requestCulture = null;

            if (isSiteMultilingual)
            {
                string permalink = getPermalink(routeData, httpContext);
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

        private string getPermalink(RouteData routeData, HttpContext httpContext)
        {
            //permalink in the url has first preference
            string permalink = (routeData.Values["permalink"] != null) ? routeData.Values["permalink"].ToString() : "";
            if (string.IsNullOrEmpty(permalink))
            {
                //if permalink is null, check for querystring
                permalink = httpContext.Request.Query["permalink"].ToString();
            }
            return permalink;
        }
    }
}
