using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PageContext = Deviser.Core.Library.PageContext;

namespace Deviser.WI.Controllers
{
    public class PageController : DeviserController
    {
        private readonly ILogger<PageController> logger;

        [ActionContext]
        public ActionContext Context { get; set; }


        ILifetimeScope container;
        IPageProvider pageProvider;
        IPageManager pageManager;
        IDeviserControllerFactory deviserControllerFactory;
        ISiteBootstrapper siteBootstrapper;
        IScopeService scopeService;

        public PageController(ILifetimeScope container, IScopeService scopeService)
        {
            this.container = container;
            logger = container.Resolve<ILogger<PageController>>();
            pageProvider = container.Resolve<IPageProvider>();
            pageManager = container.Resolve<IPageManager>();
            deviserControllerFactory = container.Resolve<IDeviserControllerFactory>();
            siteBootstrapper = container.Resolve<ISiteBootstrapper>();
            this.scopeService = scopeService;
            siteBootstrapper.InitializeSite();
        }

        public async Task<IActionResult> Index(string permalink)
        {            
            try
            {
                Page currentPage = GetPageModules(permalink);
                if (currentPage != null)
                {
                    if (pageManager.IsPageAccessible(currentPage))
                    {
                        Dictionary<string, List<Core.Common.DomainTypes.ContentResult>> moduleActionResults = await deviserControllerFactory.GetPageModuleResults(Context, currentPage.Id);
                        ViewBag.ModuleActionResults = moduleActionResults;
                        return View(currentPage);
                    }
                    else
                    {
                        return View("UnAuthorized");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Page load exception has been occured", ex);
                throw ex;
            }
            return View("NotFound");
        }

        public IActionResult Layout(string permalink)
        {
            Page currentPage = GetPageModules(permalink);            
            if (scopeService.PageContext != null)
            {
                ViewBag.Skin = Globals.AdminSkin;
                RouteData.Values.Add("permalink", permalink);
                return View(ViewBag);
            }
            return null;
        }

        public IActionResult Edit(string permalink)
        {
            Page currentPage = GetPageModules(permalink);
            if (scopeService.PageContext != null)
            {
                ViewBag.Skin = Globals.AdminSkin;
                RouteData.Values.Add("permalink", permalink);
                return View(scopeService.PageContext.CurrentPage);
            }
            return null;
        }

        private Page GetPageModules(string permalink)
        {
            Page currentPage = null;

            if (string.IsNullOrEmpty(permalink))
            {
                permalink = Globals.HomePageUrl;
                currentPage = Globals.HomePage;
            }   
            else
            {
                string requestCulture = (RouteData.Values["culture"] != null) ? RouteData.Values["culture"].ToString() : CurrentCulture.ToString().ToLower();
                permalink = $"{requestCulture}/{permalink}";
                currentPage = pageManager.GetPageByUrl(permalink, CurrentCulture.ToString());
            }

            
            PageContext pageContext = new PageContext();
            if (currentPage != null)
            {
                pageContext.CurrentPageId = currentPage.Id;
                pageContext.CurrentLink = permalink;
                currentPage.PageModule = null;
                pageContext.CurrentPage = currentPage;
                
                //Skins are not used for sometime period
                string skin = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    skin = currentPage.SkinSrc;
                else
                    skin = Globals.DefaultSkin;

                skin = skin.Replace("[G]", "~/Sites/Default/");

                //return View(skin, skinModel);
                scopeService.PageContext = pageContext; //Very important!!!
                scopeService.PageContext.CurrentCulture = CurrentCulture; //Very important!!!
                ViewBag.Skin = skin;
            }
            return currentPage;
        }

    }
}
