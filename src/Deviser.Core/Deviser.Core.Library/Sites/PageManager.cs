using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Sites
{
    public class PageManager : IPageManager
    {
        ILifetimeScope container;
        IPageProvider pageProvider;

        public PageManager(ILifetimeScope container)
        {
            this.container = container;
            this.pageProvider = container.Resolve<IPageProvider>();
        }

        public Page GetPageByUrl(string url, string locale)
        {
            Page resultPage = null;
            if (!string.IsNullOrEmpty(url))
            {
                var pageTranslation = pageProvider.GetPageTranslations(locale);
                var currentPageTranslation = pageTranslation.FirstOrDefault(p => (p != null && p.URL.ToLower() == url.ToLower()));
                if (currentPageTranslation != null)
                {
                    resultPage = pageProvider.GetPage(currentPageTranslation.PageId);
                }
            }
            return resultPage;
        }
    }
}
