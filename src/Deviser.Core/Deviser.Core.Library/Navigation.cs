using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library
{
    public class Navigation : INavigation
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        IPageProvider pageProvider;

        public Navigation(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
            pageProvider = container.Resolve<IPageProvider>();
        }

        public Page GetPageTree(int pageId)
        {
            var root = pageProvider.GetPageTree();
            return GetPageTree(root, pageId);
        }

        public Page UpdatePageTree(Page page)
        {
            try
            {
                if (page != null)
                {
                    UpdatePageTreeURL(page);
                    var resultPage = pageProvider.UpdatePageTree(page);
                    if (resultPage != null)
                        return resultPage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                logger.LogError(errorMessage, ex);
            }
            return null;
        }

        public Page UpdateSinglePage(Page page)
        {
            try
            {
                if (page != null)
                {
                    string currentURL = GetParentURL(page);
                    UpdatePageTreeURL(page, currentURL);
                    var resultPage = pageProvider.UpdatePage(page);
                    UpdateChildPages(resultPage.ChildPage);
                    if (resultPage != null)
                        return resultPage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                logger.LogError(errorMessage, ex);
            }
            return null;
        }

        public bool DeletePage(int pageId, bool forceDelete = false)
        {
            try
            {
                Page page = pageProvider.GetPage(pageId);
                if (page != null)
                {
                    page.IsDeleted = true;
                    var resultPage = pageProvider.UpdatePage(page);
                    if (resultPage != null)
                        return true;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting a page, PageId: ", pageId);
                logger.LogError(errorMessage, ex);
            }
            return false;
        }

        private Page GetPageTree(Page page, int pageId)
        {
            Page resultPage = null;
            if (page.Id == pageId)
                resultPage = page;

            if (page.ChildPage!=null)
            {
                foreach(var child in page.ChildPage)
                {
                    var childResult = GetPageTree(child, pageId);
                    if(childResult!=null)
                    {
                        resultPage = childResult;
                    }
                }
            }
            return resultPage;
        }

        private void UpdateChildPages(ICollection<Page> pages)
        {
            //Update URL of child pages, if any
            if (pages != null && pages.Count > 0)
            {
                foreach (Page child in pages)
                {
                    pageProvider.UpdatePage(child);
                    if (child.ChildPage != null)
                    {
                        UpdateChildPages(child.ChildPage);
                    }
                }
            }
        }

        private string GetParentURL(Page page)
        {
            string url = "";
            if (page != null && page.ParentId != null && page.ParentId > 0)
            {
                Page parentPage = pageProvider.GetPage((int)page.ParentId);
                var currentPageTranslation = GetPageTranslation(parentPage);
                if (currentPageTranslation != null)
                {
                    url = GetParentURL(parentPage) + "/" + currentPageTranslation.Name;
                }
            }
            return url;
        }

        private void UpdatePageTreeURL(Page page, string parentURL = "")
        {
            if (page != null)
            {
                //For new page
                if (page.Id <= 0)
                {
                    page.LastModifiedDate = page.CreatedDate = DateTime.Now;
                }

                var currentPageTranslation = GetPageTranslation(page);
                if (currentPageTranslation != null)
                {
                    parentURL += "/" + currentPageTranslation.Name.Replace(" ", "");
                    currentPageTranslation.URL = parentURL;
                    //TODO: This has to be set in client side when multilingual has been implemented
                    currentPageTranslation.Locale = Globals.FallbackLanguage;
                }

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    //foreach (var child in page.ChildPages)
                    //{
                    //    ProcessPageTranslation(child, parentURL);
                    //}

                    foreach (var child in page.ChildPage)
                    {
                        UpdatePageTreeURL(child, parentURL);
                    }
                }
            }
        }

        private void ProcessPageTranslation(Page page, string parentURL = "")
        {
            var currentPageTranslation = GetPageTranslation(page);
            if (currentPageTranslation != null)
            {
                parentURL += "/" + currentPageTranslation.Name;
                currentPageTranslation.URL = parentURL;
            }
        }

        private PageTranslation GetPageTranslation(Page page)
        {
            if (page != null)
            {
                var currentPageTranslation = page.PageTranslation.FirstOrDefault(pt => pt.Locale != null && pt.Locale.ToString() == Globals.CurrentCulture.ToString());
                if (currentPageTranslation == null)
                    currentPageTranslation = page.PageTranslation.FirstOrDefault();
                return currentPageTranslation;
            }
            return null;
        }
    }
}
