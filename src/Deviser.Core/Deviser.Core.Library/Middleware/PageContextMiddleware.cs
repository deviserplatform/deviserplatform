using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        public PageContextMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory /*,IRouter router*/)
        {
            _next = next;
            //_router = router;
            _logger = loggerFactory.CreateLogger<PageContextMiddleware>();
            _contentTypeProvider = new FileExtensionContentTypeProvider();

        }

        public async Task Invoke(HttpContext httpContext,
            IInstallationProvider installationManager,
            IServiceProvider serviceProvider)
        {
            var requestPath = httpContext.Request.Path.ToString().ToLower();
            var requestParts = requestPath.Split('.');
            if (requestParts.Length > 1 && _contentTypeProvider.Mappings.ContainsKey(requestParts[^1]))
            {
                _logger.LogInformation("Handling request: " + httpContext.Request.Path);
                await _next.Invoke(httpContext);
                _logger.LogInformation("Finished handling request.");
                return;
            }

            var pageContext = new PageContext();
            var isInstalled = installationManager.IsPlatformInstalled;

            if (isInstalled)
            {

                var pageManager = serviceProvider.GetService<IPageManager>();
                var pageRepository = serviceProvider.GetService<IPageRepository>();
                var moduleRepository = serviceProvider.GetService<IModuleRepository>();
                var settingManager = serviceProvider.GetService<ISettingManager>();
                var languageRepository = serviceProvider.GetService<ILanguageRepository>();
                var scopeService = serviceProvider.GetService<IScopeService>();

                var routeData = httpContext.GetRouteData();
                var activeLanguages = languageRepository.GetActiveLanguages();
                pageContext.IsMultilingual = languageRepository.IsMultilingual();
                pageContext.SiteSettingInfo = settingManager.GetSiteSetting();
                var currentCulture = GetCurrentCulture(routeData, httpContext, activeLanguages, pageContext.IsMultilingual, pageContext.SiteSettingInfo.SiteLanguage.CultureCode);
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentCulture;
                pageContext.CurrentCulture = currentCulture;
                var homePageId = pageContext.SiteSettingInfo.HomePageId;

                pageContext.HomePage = pageManager.GetPageAndTranslation(homePageId);


                if (!httpContext.Request.Path.Value.Contains("/api") && routeData.Values.Values.Count > 0)
                {
                    try
                    {
                        //This method initilizes the PageContext based on the RouteData or Query "permalink"
                        //permalink in the url has first preference
                        var permalink = GetPermalink(routeData, httpContext);
                        Page currentPage = null;
                        if (string.IsNullOrEmpty(permalink))
                        {
                            currentPage = pageContext.HomePage;
                        }
                        else
                        {
                            var requestCulture = currentCulture.ToString().ToLower();

                            if (permalink.Contains(requestCulture) && !pageContext.IsMultilingual)
                                permalink = permalink.Replace(requestCulture + "/", "");

                            currentPage = pageManager.GetPageAndTranslationByUrl(permalink, currentCulture.ToString());
                        }

                        InitPageContext(httpContext, pageManager, scopeService, currentPage, pageContext);
                        InitModuleContext(httpContext, pageRepository, moduleRepository, scopeService, routeData);
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = string.Format("Error occured while initializing site ");
                        _logger.LogError(errorMessage, ex);
                    }
                }
                else if (httpContext.Request.Headers.ContainsKey("currentPageId") && Guid.TryParse(httpContext.Request.Headers["currentPageId"], out var currentPageId))
                {
                    var currentPage = pageManager.GetPageAndTranslation(currentPageId);
                    InitPageContext(httpContext, pageManager, scopeService, currentPage, pageContext);
                }
            }

            _logger.LogInformation("Handling request: " + httpContext.Request.Path);
            await _next.Invoke(httpContext);
            //deviserDbContext.RegisterForDispose(httpContext);
            _logger.LogInformation("Finished handling request.");
        }

        private static void InitModuleContext(HttpContext httpContext, 
            IPageRepository pageRepository,
            IModuleRepository moduleRepository,
            IScopeService scopeService, 
            RouteData routeData)
        {
            Module module = null;
            ModuleContext moduleContext = null;
            var pageModuleId = Guid.Empty;

            if (routeData.Values.TryGetValue("area", out var moduleName))
            {
                module = moduleRepository.GetModule((string)moduleName);
            }

            if (routeData.Values.TryGetValue("pageModuleId", out var objPageModuleId))
            {
                Guid.TryParse(objPageModuleId.ToString(), out pageModuleId);
            }

            if (module == null) return;

            if (pageModuleId == Guid.Empty)
            {
                moduleContext = new ModuleContext(module);
            }
            else
            {
                var pageModule = pageRepository.GetPageModule(pageModuleId);
                moduleContext = new ModuleContext(module, pageModule);
            }


            if (!httpContext.Items.ContainsKey("ModuleContext"))
            {
                httpContext.Items.Add("ModuleContext", moduleContext);
            }
            else
            {
                httpContext.Items["ModuleContext"] = moduleContext;
            }

            scopeService.ModuleContext = moduleContext;
        }

        private static void InitPageContext(HttpContext httpContext, IPageManager pageManager, IScopeService scopeService,
            Page currentPage, PageContext pageContext)
        {
            pageContext.CurrentPageId = currentPage.Id;
            pageContext.CurrentUrl = currentPage.PageTranslation.First(p => p.Locale.Equals(pageContext.CurrentCulture.ToString(), StringComparison.InvariantCultureIgnoreCase)).URL;
            pageContext.CurrentPage = currentPage;

            if (!httpContext.Items.ContainsKey("PageContext"))
            {
                httpContext.Items.Add("PageContext", pageContext);
            }
            else
            {
                httpContext.Items["PageContext"] = pageContext;
            }

            scopeService.PageContext = pageContext;
        }

        /// <summary>
        /// Identifies the current culture based on the request information and SiteSettings:
        /// 1. Checks whether the site is multilingual
        /// 1.1 If Yes:
        /// 1.1.1 Look's for culture from URL or from browser language
        /// 1.2 If request culture matches, assign them else use the site's default language
        /// </summary>
        /// <param name="routeData"></param>
        /// <param name="httpContext"></param>
        /// <param name="activeLanguages"></param>
        /// <param name="isSiteMultilingual"></param>
        /// <returns></returns>
        private CultureInfo GetCurrentCulture(RouteData routeData, HttpContext httpContext, IList<Language> activeLanguages, bool isSiteMultilingual, string siteDefaultCulture)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature.RequestCulture.UICulture;

            //if (isSiteMultilingual)
            //{
            //    var permalink = GetPermalink(routeData, httpContext);
            //    var match = Regex.Match(permalink, @"[a-z]{2}-[a-z]{2}", RegexOptions.IgnoreCase);
            //    requestCulture = match.Success ? new CultureInfo(match.Value) : requestCultureFeature.RequestCulture.UICulture;
            //    var hasRequestCulture = activeLanguages.Any(l =>
            //        l.CultureCode.Equals(requestCulture.Name, StringComparison.InvariantCultureIgnoreCase));
            //    if (!hasRequestCulture)
            //    {
            //        requestCulture = new CultureInfo(siteDefaultCulture);
            //    }
            //}
            //else
            //{
            //    requestCulture = new CultureInfo(activeLanguages.First().CultureCode);
            //}

            Globals.CurrentCulture = requestCulture;
            return requestCulture;
        }

        private string GetPermalink(RouteData routeData, HttpContext httpContext)
        {
            //permalink in the url has first preference
            var permalink = (routeData.Values["permalink"] != null) ? routeData.Values["permalink"].ToString() : "";
            if (string.IsNullOrEmpty(permalink))
            {
                //if permalink is null, check for querystring
                permalink = httpContext.Request.Query["permalink"].ToString();
            }
            permalink = Uri.UnescapeDataString(permalink);
            return permalink;
        }
    }
}
