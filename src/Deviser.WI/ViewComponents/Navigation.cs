using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.ViewComponents
{
    [ViewComponent(Name = "Navigation")]
    public class Navigation : DeviserViewComponent
    {
        //Logger
        private readonly ILogger<Navigation> logger;
        IPageProvider pageProvider;

        public Navigation(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<Navigation>>();
            pageProvider = container.Resolve<IPageProvider>();
        }

        public IViewComponentResult Invoke(bool isAdmin = false)
        {
            var pages = pageProvider.GetPages();
            foreach (var page in pages)
            {
                FilterPage(page, isAdmin);
            }
            return View(pages);
        }

        private void FilterPage(Page page, bool isAdmin)
        {
            if (page != null)
            {
                if (page.Id == AppContext.CurrentPageId)
                {
                    page.IsActive = true;
                }

                //Filter admin pages
                if (page.ChildPage != null && !isAdmin)
                    page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                    {
                        if(child.Id == AppContext.CurrentPageId)
                        {
                            page.IsBreadCrumb = true;
                        }
                                                
                        FilterPage(child, isAdmin);
                    }
                }
            }
        }

        //private PageTranslation GetPageTranslation(Page page)
        //{
        //    if (page != null)
        //    {
        //        var currentPageTranslation = page.PageTranslation.FirstOrDefault(pt => pt.Locale != null && pt.Locale.ToString() == Globals.CurrentCulture.ToString());
        //        if (currentPageTranslation == null)
        //            currentPageTranslation = page.PageTranslation.FirstOrDefault();
        //        return currentPageTranslation;
        //    }
        //    return null;
        //}
    }
}
