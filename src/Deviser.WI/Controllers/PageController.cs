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
using Deviser.Core.Library.Modules;

namespace Deviser.WI.Controllers
{
    public class PageController : DeviserController
    {
        private readonly ILogger<PageController> logger;

        [ActionContext]
        public ActionContext Context { get; set; }


        private ILifetimeScope container;
        private IPageProvider pageProvider;
        private IPageManager pageManager;
        private IDeviserControllerFactory deviserControllerFactory;
        private ISiteBootstrapper siteBootstrapper;
        private IScopeService scopeService;
        private IContentManager contentManager;
        private IModuleManager moduleManager;

        public PageController(ILifetimeScope container, IScopeService scopeService)
        {
            this.container = container;
            logger = container.Resolve<ILogger<PageController>>();
            pageProvider = container.Resolve<IPageProvider>();
            pageManager = container.Resolve<IPageManager>();
            deviserControllerFactory = container.Resolve<IDeviserControllerFactory>();
            siteBootstrapper = container.Resolve<ISiteBootstrapper>();
            contentManager = container.Resolve<IContentManager>();
            moduleManager = container.Resolve<IModuleManager>();
            this.scopeService = scopeService;
            siteBootstrapper.InitializeSite();
        }

        public async Task<IActionResult> Index(string permalink)
        {
            try
            {
                Page currentPage = scopeService.PageContext.CurrentPage;
                FilterPageElements(currentPage);
                if (currentPage != null)
                {
                    if (scopeService.PageContext.HasPageViewPermission)
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
            Page currentPage = scopeService.PageContext.CurrentPage;
            FilterPageElements(currentPage);
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
            Page currentPage = scopeService.PageContext.CurrentPage;
            FilterPageElements(currentPage);
            if (currentPage != null && scopeService.PageContext != null)
            {
                if (scopeService.PageContext.HasPageEditPermission)
                {
                    ViewBag.Skin = Globals.AdminSkin;
                    RouteData.Values.Add("permalink", permalink);
                    return View(scopeService.PageContext.CurrentPage);
                }
                else
                {
                    return View("UnAuthorized");
                }

            }
            return View("NotFound");
        }

        [HttpGet]
        [Route("[controller]/[action]/{pageModuleId}/{moduleActionId}")]
        public IActionResult EditModule(string currentLink, Guid pageModuleId, Guid moduleActionId)
        {
            if (pageModuleId != Guid.Empty)
            {
                try
                {
                    var pageModule = pageProvider.GetPageModule(pageModuleId); //It referes PageModule's View ModuleActionType
                    
                    if (pageModule == null)
                        return NotFound();

                    //if (moduleManager.HasEditPermission(pageModule))
                    if (scopeService.PageContext.HasPageEditPermission)
                    {
                        object result = deviserControllerFactory.GetModuleEditResult(Context, pageModule, moduleActionId).Result;
                        ViewBag.result = result;
                        return View(result);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Module load exception has been occured", ex);
                    throw ex;
                }
            }
            else
            {
                return BadRequest("Invalid module id");
            }

        }

        //private Page InitPageContext(string permalink)
        //{
        //    Page currentPage = null;

        //    if (string.IsNullOrEmpty(permalink))
        //    {
        //        permalink = Globals.HomePageUrl;
        //        currentPage = Globals.HomePage;
        //    }
        //    else
        //    {
        //        string requestCulture = (RouteData.Values["culture"] != null) ? RouteData.Values["culture"].ToString() : CurrentCulture.ToString().ToLower();

        //        if (!permalink.Contains(requestCulture))
        //            permalink = $"{requestCulture}/{permalink}";

        //        currentPage = pageManager.GetPageByUrl(permalink, CurrentCulture.ToString());
        //    }
        //    PageContext pageContext = new PageContext();
        //    pageContext.CurrentPageId = currentPage.Id;
        //    pageContext.CurrentUrl = permalink;
        //    pageContext.CurrentPage = currentPage;
        //    pageContext.HasPageViewPermission = pageManager.HasViewPermission(currentPage);
        //    pageContext.HasPageEditPermission = pageManager.HasEditPermission(currentPage);
        //    scopeService.PageContext = pageContext; //Very important!!!
        //    scopeService.PageContext.CurrentCulture = CurrentCulture; //Very important!!!
        //    return currentPage;
        //}

        private void FilterPageElements(Page currentPage)
        {
            if (currentPage != null)
            {

                if (currentPage.PageContent != null && currentPage.PageContent.Count > 0)
                {
                    //Filter pageContents where current user has view permissions
                    var filteredPageContents = currentPage.PageContent.Where(pc => contentManager.HasViewPermission(pc)).ToList();
                    currentPage.PageContent = filteredPageContents;
                }

                if (currentPage.PageModule != null && currentPage.PageModule.Count > 0)
                {
                    //Filter pageContents where current user has view permissions
                    var filteredPageModules = currentPage.PageModule.Where(pm => moduleManager.HasViewPermission(pm)).ToList();
                    currentPage.PageModule = filteredPageModules;
                }

                //Skins are not used for sometime period
                string skin = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    skin = currentPage.SkinSrc;
                else
                    skin = Globals.DefaultSkin;

                skin = skin.Replace("[G]", "~/Sites/Default/");

                ViewBag.Skin = skin;
            }
        }
    }
}
