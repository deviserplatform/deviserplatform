using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
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
            contentManager = container.Resolve<IContentManager>();
            moduleManager = container.Resolve<IModuleManager>();
            this.scopeService = scopeService;
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
                        Dictionary<string, List<Core.Common.DomainTypes.ContentResult>> moduleActionResults = await deviserControllerFactory.GetPageModuleResults(Context);
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
                ViewBag.Theme = Globals.AdminTheme;
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
                    ViewBag.Theme = Globals.AdminTheme;
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

                //Themes are not used for sometime period
                string theme = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    theme = currentPage.SkinSrc;
                else
                    theme = Globals.DefaultTheme;

                theme = theme.Replace("[G]", "~/Sites/Default/");

                ViewBag.Theme = theme;
            }
        }
    }
}
