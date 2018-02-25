using Autofac;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Http;
using Deviser.Core.Library.Sites;

using Deviser.Core.Library.Services;
using Deviser.Core.Library.Multilingual;

namespace Deviser.Core.Library
{
    public class Navigation : PageManager, INavigation
    {
        //Logger
        private readonly ILogger<LayoutRepository> _logger;
        private readonly ILanguageRepository _languageRepository;
        private readonly IScopeService _scopeService;

        private Page activePage = null;
        private List<Page> breadcrumbs = null;

        #region Public Methods
        public Navigation(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<LayoutRepository>>();
            _languageRepository = container.Resolve<ILanguageRepository>();
            _scopeService = container.Resolve<IScopeService>();
        }

        public Page GetPageTree()
        {
            try
            {
                var root = _pageRepository.GetPageTree();
                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTree", ex);
            }
            return null;
        }

        public Page GetPageTree(Guid parentId)
        {
            try
            {
                var root = _pageRepository.GetPageTree();
                return GetPageTree(root, parentId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTree by id", ex);
            }
            return null;
        }

        public Page GetPageTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid())
        {
            try
            {
                Page root;
                if (parentId != Guid.Empty)
                {
                    root = GetPageTree(parentId);
                }
                else
                {
                    root = _pageRepository.GetPageTree();
                }

                Func<Page, bool> predicate = null;

                //system page filter
                if (systemFilter == SystemPageFilter.PublicOnly)
                {
                    //page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();
                    predicate = p => !p.IsSystem && !p.IsDeleted && p.IsIncludedInMenu && HasViewPermission(p);
                }
                else if (systemFilter == SystemPageFilter.SystemOnly)
                {
                    //page.ChildPage = page.ChildPage.Where(p => p.IsSystem).ToList();
                    predicate = p => p.IsSystem && !p.IsDeleted && p.IsIncludedInMenu && HasViewPermission(p);
                }


                FilterPage(root, currentPageId, predicate);
                SetBreadCrumb(activePage);
                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTree with filter", ex);
            }
            return null;
        }
        
        public List<Page> GetPages()
        {
            try
            {
                var result = _pageRepository.GetPages();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all pages in list", ex);
            }
            return null;
        }

        public List<Page> GetBreadCrumbs(Guid currentPageId)
        {
            try
            {
                Page root = _pageRepository.GetPageTree();
                FilterPage(root, currentPageId);
                breadcrumbs = new List<Page>();
                SetBreadCrumb(activePage);
                return breadcrumbs.OrderBy(p => p.PageLevel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting breadcrumbs", ex);
                throw;
            }
        }

        public Page CreatePage(Page page)
        {
            try
            {
                var pageTranslation = page.PageTranslation;
                page.PageTranslation = null;
                var newPage = _pageRepository.CreatePage(page);
                if (newPage != null)
                {
                    page.Id = newPage.Id;
                    page.PageTranslation = pageTranslation;
                    var result = UpdateSinglePage(page);
                    return result;
                }                    
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page, PageId: ", page.Id), ex);
            }
            return null;
        }

        public Page UpdatePageTree(Page page)
        {
            try
            {
                if (page != null)
                {
                    var parentURLs = InitParentUrls();
                    UpdatePageTreeURL(page, parentURLs);
                    var resultPage = _pageRepository.UpdatePageTree(page);
                    if (resultPage != null)
                        return resultPage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                _logger.LogError(errorMessage, ex);
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
                    var resultPage = _pageRepository.UpdatePage(page);

                    //Update admin permissions
                    var adminPermissions = new List<PagePermission>();
                    adminPermissions.Add(new PagePermission
                    {
                        PageId = page.Id,
                        RoleId = Globals.AdministratorRoleId,
                        PermissionId = Globals.PageViewPermissionId,
                    });

                    adminPermissions.Add(new PagePermission
                    {
                        PageId = page.Id,
                        RoleId = Globals.AdministratorRoleId,
                        PermissionId = Globals.PageEditPermissionId,
                    });

                    adminPermissions = _pageRepository.AddPagePermissions(adminPermissions);

                    if (resultPage.PagePermissions == null)
                    {
                        resultPage.PagePermissions = adminPermissions;
                    }
                    else
                    {
                        adminPermissions.AddRange(resultPage.PagePermissions);
                        resultPage.PagePermissions = adminPermissions;
                    }

                    UpdateChildPages(resultPage.ChildPage);
                    if (resultPage != null)
                        return resultPage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating a page, PageId: ", page.Id);
                _logger.LogError(errorMessage, ex);
            }
            return null;
        }

        public bool DeletePage(Guid pageId, bool forceDelete = false)
        {
            try
            {
                Page page = _pageRepository.GetPage(pageId);
                if (page != null)
                {
                    page.IsDeleted = true;
                    var resultPage = _pageRepository.UpdatePage(page);
                    if (resultPage != null)
                        return true;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting a page, PageId: ", pageId);
                _logger.LogError(errorMessage, ex);
            }
            return false;
        }

        public string NavigateUrl(string pageId, string locale = null)
        {
            if (!string.IsNullOrEmpty(pageId))
            {
                try
                {
                    Guid id = Guid.Parse(pageId);
                    var page = _pageRepository.GetPage(id);
                    return NavigateUrl(page, locale);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occured while getting page url by pageId and locale", ex);
                }
            }
            return string.Empty;
        }

        public string NavigateUrl(Guid pageId, string locale = null)
        {
            if(pageId!= Guid.Empty)
            {
                try
                {
                    var page = _pageRepository.GetPage(pageId);
                    return NavigateUrl(page, locale);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occured while getting page url by pageId and locale", ex);
                }
            }
            return string.Empty;
        }

        public string NavigateUrl(Page page, string locale = null)
        {
            if (page != null)
            {
                try
                {
                    if (locale == null)
                        locale = _scopeService.PageContext.CurrentCulture.ToString().ToLower();
                    var translation = page.PageTranslation.FirstOrDefault(t => t.Locale.ToLower() == locale);

                    return translation != null ? _scopeService.PageContext.SiteRoot + translation.URL : "";
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occured while getting page url by page and locale", ex);
                }
            }
            return string.Empty;
        }
        #endregion

        #region Private Methods

        private Dictionary<string, string> InitParentUrls()
        {
            var activeLanguages = _languageRepository.GetActiveLanguages();
            Dictionary<string, string> parentURLs = new Dictionary<string, string>();
            if (activeLanguages != null && activeLanguages.Count > 1)
            {
                foreach (var lang in activeLanguages)
                {
                    parentURLs.Add(lang.CultureCode, lang.CultureCode.ToLower());
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
            foreach (var kv in parentUrls)
            {
                result[kv.Key] = kv.Value;
            }
            return result;
        }

        private Page GetPageTree(Page page, Guid pageId)
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
                    _pageRepository.UpdatePage(child);
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
            if (page != null && page.ParentId != null && page.ParentId != Guid.Empty)
            {
                Page parentPage = _pageRepository.GetPage((Guid)page.ParentId);
                var parentURLs = GetParentURL(parentPage);
                if (parentPage.PageTranslation != null && parentPage.PageTranslation.Count > 0)
                {
                    foreach (var pageTranslation in parentPage.PageTranslation)
                    {
                        if (parentURLs.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
                        {
                            //parent page has translation for current locale/culturecode
                            URLs[pageTranslation.Locale] = parentURLs[pageTranslation.Locale] + (!string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
                        }
                        else
                        {
                            URLs[pageTranslation.Locale] = parentURLs[Globals.FallbackLanguage] + (!string.IsNullOrEmpty(parentURLs[Globals.FallbackLanguage]) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
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
                if (page.Id == Guid.Empty)
                {
                    page.LastModifiedDate = page.CreatedDate = DateTime.Now;
                }

                if (page.PageTranslation != null && page.PageTranslation.Count > 0)
                {
                    string fallbackParentURL = parentURLs[Globals.FallbackLanguage];
                    foreach (var pageTranslation in page.PageTranslation)
                    {
                        if (parentURLs.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
                        {
                            parentURLs[pageTranslation.Locale] += "/" + pageTranslation.Name.Replace(" ", "");
                            var pageUrl = parentURLs[pageTranslation.Locale];
                            pageTranslation.URL = getUniqueUrl(pageTranslation, pageUrl, page.Id);
                        }
                        else
                        {
                            //Parent page is not yet translated, therefore taking parent url from fallbacklanguage

                            parentURLs[pageTranslation.Locale] = (!string.IsNullOrEmpty(fallbackParentURL) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
                            var pageUrl = parentURLs[pageTranslation.Locale];
                            pageTranslation.URL = getUniqueUrl(pageTranslation, pageUrl, page.Id);
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

        private string getUniqueUrl(PageTranslation pageTranslation, string pageUrl, Guid pageId)
        {
            var duplicateTranslation = _pageRepository.GetPageTranslation(pageUrl.ToLower());

            if (duplicateTranslation != null && duplicateTranslation.PageId != pageId)
            {
                while (duplicateTranslation != null && duplicateTranslation.URL.ToLower() == pageUrl.ToLower())
                {
                    pageUrl += "1";
                    pageTranslation.Name += "1";
                    duplicateTranslation = _pageRepository.GetPageTranslation(pageUrl);
                }
            }
            return pageUrl;
        }

        private void FilterPage(Page page, Guid currentPageId, Func<Page, bool> predicate = null)
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

        #endregion
    }
}
