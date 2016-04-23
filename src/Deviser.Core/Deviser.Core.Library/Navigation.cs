using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
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

        private IPageProvider pageProvider;
        private ILanguageProvider languageProvider;
        private Page activePage = null;
        private List<Page> breadcrumbs = null;

        public Navigation(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
            pageProvider = container.Resolve<IPageProvider>();
            languageProvider = container.Resolve<ILanguageProvider>();
        }

        public Page GetPageTree(int parentId)
        {
            var root = pageProvider.GetPageTree();
            return GetPageTree(root, parentId);
        }

        public Page GetPageTree(int currentPageId, SystemPageFilter systemFilter, int parentId = 0)
        {
            Page root;
            if (parentId > 0)
            {
                root = GetPageTree(parentId);
            }
            else
            {
                root = pageProvider.GetPageTree();
            }

            Func<Page, bool> predicate = null;

            //system page filter
            if (systemFilter == SystemPageFilter.PublicOnly)
            {
                //page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();
                predicate = p => !p.IsSystem;
            }
            else if (systemFilter == SystemPageFilter.SystemOnly)
            {
                //page.ChildPage = page.ChildPage.Where(p => p.IsSystem).ToList();
                predicate = p => p.IsSystem;
            }


            FilterPage(root, currentPageId, predicate);
            SetBreadCrumb(activePage);
            return root;
        }

        public List<Page> GetBreadCrumbs(int currentPageId)
        {
            Page root = pageProvider.GetPageTree();
            FilterPage(root, currentPageId);
            breadcrumbs = new List<Page>();
            SetBreadCrumb(activePage);
            return breadcrumbs.OrderBy(p => p.PageLevel).ToList();
        }

        public Page UpdatePageTree(Page page)
        {
            try
            {
                if (page != null)
                {
                    var parentURLs = InitParentUrls();
                    UpdatePageTreeURL(page, parentURLs);
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

                    var currentURLs = GetParentURL(page);
                    UpdatePageTreeURL(page, currentURLs);
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

        private Dictionary<string, string> InitParentUrls()
        {
            var activeLanguages = languageProvider.GetLanguages();
            Dictionary<string, string> parentURLs = new Dictionary<string, string>();
            if (activeLanguages != null && activeLanguages.Count > 0)
            {
                foreach (var lang in activeLanguages)
                {
                    parentURLs.Add(lang.CultureCode, "");
                }
            }
            else
            {
                parentURLs.Add(Globals.FallbackLanguage, "");
            }
            return parentURLs;
        }

        private Dictionary<string, string> CopyParentUrls(Dictionary<string, string> parentUrls)
        {
            var result = InitParentUrls();
            foreach(var kv in parentUrls)
            {
                result[kv.Key] = kv.Value;
            }
            return result;
        }

        private Page GetPageTree(Page page, int pageId)
        {
            Page resultPage = null;
            if (page.Id == pageId)
                resultPage = page;

            if (page.ChildPage != null)
            {
                foreach (var child in page.ChildPage)
                {
                    var childResult = GetPageTree(child, pageId);
                    if (childResult != null)
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

        private Dictionary<string, string> GetParentURL(Page page)
        {
            //string url = "";
            var URLs = InitParentUrls();
            if (page != null && page.ParentId != null && page.ParentId > 0)
            {
                Page parentPage = pageProvider.GetPage((int)page.ParentId);
                var parentURLs = GetParentURL(parentPage);
                if (parentPage.PageTranslation != null && parentPage.PageTranslation.Count > 0)
                {
                    foreach (var pageTranslation in parentPage.PageTranslation)
                    {
                        if (parentURLs.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
                        {
                            //parent page has translation for current locale/culturecode
                            URLs[pageTranslation.Locale] = parentURLs[pageTranslation.Locale] + "/" + pageTranslation.Name.Replace(" ", "");
                        }
                        else
                        {
                            URLs[pageTranslation.Locale] = parentURLs[Globals.FallbackLanguage] + "/" + pageTranslation.Name.Replace(" ", "");
                        }
                    }
                }

                //var currentPageTranslation = GetPageTranslation(parentPage);
                //if (currentPageTranslation != null)
                //{
                //    url = GetParentURL(parentPage) + "/" + currentPageTranslation.Name;
                //}
            }
            return URLs;
        }

        private void UpdatePageTreeURL(Page page, Dictionary<string, string> parentURLs)
        {
            if (page != null)
            {

                //For new page
                if (page.Id <= 0)
                {
                    page.LastModifiedDate = page.CreatedDate = DateTime.Now;
                }

                if (page.PageTranslation != null && page.PageTranslation.Count > 0)
                {
                    string fallbackParentURL = parentURLs[Globals.FallbackLanguage];
                    foreach (var pageTranslation in page.PageTranslation)
                    {
                        if (!string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
                        {
                            parentURLs[pageTranslation.Locale] += "/" + pageTranslation.Name.Replace(" ", "");
                            pageTranslation.URL = parentURLs[pageTranslation.Locale];
                        }
                        else
                        {
                            //Parent page is not yet translated, therefore taking parent url from fallbacklanguage
                            parentURLs[pageTranslation.Locale] = fallbackParentURL + "/" + pageTranslation.Name.Replace(" ", "");
                            pageTranslation.URL = parentURLs[pageTranslation.Locale];
                        }

                    }
                }

                //var currentPageTranslation = GetPageTranslation(page);
                //if (currentPageTranslation != null)
                //{
                //    parentURL += "/" + currentPageTranslation.Name.Replace(" ", "");
                //    currentPageTranslation.URL = parentURL;
                //    //TODO: This has to be set in client side when multilingual has been implemented
                //    currentPageTranslation.Locale = Globals.FallbackLanguage;
                //}

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    //foreach (var child in page.ChildPages)
                    //{
                    //    ProcessPageTranslation(child, parentURL);
                    //}

                    foreach (var child in page.ChildPage)
                    {
                        //if (page.PageLevel == 0)
                        var pUrl = CopyParentUrls(parentURLs);

                        UpdatePageTreeURL(child, pUrl);
                    }
                }
            }
        }

        //private void ProcessPageTranslation(Page page, string parentURL = "")
        //{
        //    var currentPageTranslation = GetPageTranslation(page);
        //    if (currentPageTranslation != null)
        //    {
        //        parentURL += "/" + currentPageTranslation.Name;
        //        currentPageTranslation.URL = parentURL;
        //    }
        //}

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

        private void FilterPage(Page page, int currentPageId, Func<Page, bool> predicate = null)
        {
            if (page != null)
            {
                if (page.Id == currentPageId)
                {
                    page.IsActive = true;
                    activePage = page;
                }

                //Page filter
                if (page.ChildPage != null)
                {
                    if (predicate != null)
                        page.ChildPage = page.ChildPage.Where(predicate).ToList();

                    page.ChildPage = page.ChildPage.OrderBy(p => p.PageOrder).ToList();
                }

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                    {
                        FilterPage(child, currentPageId, predicate);
                    }
                }
            }
        }

        private void SetBreadCrumb(Page activePage)
        {
            if (activePage != null)
            {
                activePage.IsBreadCrumb = true;
                if (breadcrumbs != null)
                {
                    breadcrumbs.Add(activePage);
                }

                if (activePage.Parent != null)
                {
                    SetBreadCrumb(activePage.Parent);
                }
            }
        }
    }
}
