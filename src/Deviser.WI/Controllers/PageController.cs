using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Sites;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers
{
    public class PageController : DeviserController
    {
        private readonly ILogger<PageController> logger;

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
            siteBootstrapper.UpdateSiteSettings();
        }

        public async Task<IActionResult> Index(string permalink)
        {
            if (string.IsNullOrEmpty(permalink))
                permalink =  Globals.HomePage.PageTranslation.FirstOrDefault(t => t.Locale == CurrentCulture.ToString()).URL;
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
            }            
            return null;
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
            if (!permalink.StartsWith("/"))
                permalink = "/" + permalink;

            Page currentPage = pageManager.GetPageByUrl(permalink, CurrentCulture.ToString());
            AppContext appContext = new AppContext();
            if (currentPage != null)
            {
                appContext.CurrentPageId = currentPage.Id;
                appContext.CurrentLink = permalink;
                currentPage.PageModule = null;                
                appContext.CurrentPage = currentPage;
                Dictionary<string, List<Core.Library.DomainTypes.ContentResult>> moduleActionResults = await deviserControllerFactory.GetPageModuleResults(ActionContext, currentPage.Id);
                //Skins are not used for sometime period
                string skin = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    skin = currentPage.SkinSrc;
                else
                    skin = "[G]Skins/Skyline/Home.cshtml";

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
