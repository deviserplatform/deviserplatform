using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppContext = Deviser.Core.Library.AppContext;

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

        public PageController(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<PageController>>();
            pageProvider = container.Resolve<IPageProvider>();
            pageManager = container.Resolve<IPageManager>();
            deviserControllerFactory = container.Resolve<IDeviserControllerFactory>();
            siteBootstrapper = container.Resolve<ISiteBootstrapper>();
            siteBootstrapper.InitializeSite();
        }

        public async Task<IActionResult> Index(string permalink)
        {            
            try
            {
                Page currentPage = await GetPageModules(permalink);
                if (currentPage != null)
                {
                    return View(currentPage);
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
            //Page currentPage = await GetPageModules(permalink);
            if (AppContext != null)
            {
                ViewBag.Skin = Globals.AdminSkin;
                ViewBag.AppContext = AppContext;
                return View(ViewBag);
            }
            return null;
        }

        public IActionResult Edit(string permalink)
        {
            if (AppContext != null)
            {
                ViewBag.Skin = Globals.AdminSkin;
                ViewBag.AppContext = AppContext;
                return View(AppContext.CurrentPage);
            }
            return null;
        }

        private async Task<Page> GetPageModules(string permalink)
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

            
            AppContext appContext = new AppContext();
            if (currentPage != null)
            {
                appContext.CurrentPageId = currentPage.Id;
                appContext.CurrentLink = permalink;
                currentPage.PageModule = null;
                appContext.CurrentPage = currentPage;
                Dictionary<string, List<Core.Library.DomainTypes.ContentResult>> moduleActionResults = await deviserControllerFactory.GetPageModuleResults(Context, currentPage.Id);
                //Skins are not used for sometime period
                string skin = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    skin = currentPage.SkinSrc;
                else
                    skin = Globals.DefaultSkin;

                skin = skin.Replace("[G]", "~/Sites/Default/");

                //var contentModuleHtml = System.Web.Mvc.Html.renderac

                /*DeviserWI.Modules.Content.Controllers.DefaultController contentController = new DeviserWI.Modules.Content.Controllers.DefaultController();
                ActionResult result = contentController.Index();

                foreach (var pageModule in currentPage.PageModules)
                {
                    skinModel.Add(pageModule.PaneName, result);
                }*/


                //return View(skin, skinModel);
                AppContext = appContext;
                ViewBag.AppContext = AppContext;
                ViewBag.ModuleActionResults = moduleActionResults;
                ViewBag.Skin = skin;
            }
            return currentPage;
        }

    }
}
