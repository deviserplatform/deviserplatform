using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Controllers;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers
{
    public class PageController : DeviserController
    {
        ILifetimeScope container;
        IPageProvider pageProvider;
        IDeviserControllerFactory deviserControllerFactory;

        public PageController(ILifetimeScope container)
        {
            this.container = container;
            this.pageProvider = container.Resolve<IPageProvider>();
            this.deviserControllerFactory = container.Resolve<IDeviserControllerFactory>();
        }

        public async Task<IActionResult> Index(string permalink)
        {
            permalink = "/" + permalink;
            Page currentPage;
            var pageTranslation = pageProvider.GetPageTranslations(CurrentCulture.ToString());

            var currentPageTranslation = pageTranslation.FirstOrDefault(p => (p != null && p.URL.ToLower() == permalink.ToLower()));

            if (currentPageTranslation != null)
            {
                currentPage = pageProvider.GetPage(currentPageTranslation.PageId);
                AppContext.CurrentPageId = currentPage.Id;
                AppContext.CurrentLink = permalink;
                AppContext.CurrentPage = currentPage;
                Dictionary<string, string> moduleActionResults = await deviserControllerFactory.GetPageModuleResults(ActionContext, currentPage.Id);
                //Skins are not used for sometime period
                string skin = "";
                if (!string.IsNullOrEmpty(currentPage.SkinSrc))
                    skin = currentPage.SkinSrc;
                else
                    skin = "[G]Skins/Skyline/Home.cshtml";

                skin = skin.Replace("[G]", "~/Portals/_default/");

                //var contentModuleHtml = System.Web.Mvc.Html.renderac

                /*DeviserWI.Modules.Content.Controllers.DefaultController contentController = new DeviserWI.Modules.Content.Controllers.DefaultController();
                ActionResult result = contentController.Index();

                foreach (var pageModule in currentPage.PageModules)
                {
                    skinModel.Add(pageModule.PaneName, result);
                }*/


                //return View(skin, skinModel);
                ViewBag.AppContext = AppContext;
                ViewBag.ModuleActionResults = moduleActionResults;
                ViewBag.Skin = skin;
                return View(currentPage);
            }

            return null;
        }


    }
}
