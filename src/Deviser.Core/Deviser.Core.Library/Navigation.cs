using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IScopeService _scopeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Page activePage = null;
        private IList<Page> breadcrumbs = null;

        #region Public Methods
        public Navigation(ILogger<LayoutRepository> logger,
            ILanguageRepository languageRepository,
            IMapper mapper,
            IScopeService scopeService,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor)
            : base(serviceProvider)
        {
            _logger = logger;
            _languageRepository = languageRepository;
            _mapper = mapper;
            _scopeService = scopeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Page GetPageTree()
        {
            var root = _pageRepository.GetPageTree();
            return root;
        }

        public Page GetPageTree(Guid parentId)
        {
            var root = _pageRepository.GetPageTree();
            return GetPageTreeFrom(root, parentId);
        }

        public Page GetPageTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid())
        {
            Page root;
            root = parentId != Guid.Empty ? GetPageTree(parentId) : _pageRepository.GetPageTree();

            Func<Page, bool> predicate = null;

            //system page filter
            if (systemFilter == SystemPageFilter.PublicOnly)
            {
                //page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();
                predicate = p => !p.IsSystem && p.IsActive && p.IsIncludedInMenu && HasViewPermission(p);
            }
            else if (systemFilter == SystemPageFilter.SystemOnly)
            {
                //page.ChildPage = page.ChildPage.Where(p => p.IsSystem).ToList();
                predicate = p => p.IsSystem && p.IsActive && p.IsIncludedInMenu && HasViewPermission(p);
            }


            FilterPage(root, currentPageId, predicate);
            SetBreadCrumb(currentPageId, root);
            return root;
        }

        public IList<Page> GetPages()
        {
            var result = _pageRepository.GetPagesFlat();
            return result;
        }

        public IList<Page> GetPublicPages()
        {
            var allPages = GetPages();
            var publicPages = allPages.Where(p => HasViewPermission(p)).ToList();
            return publicPages;
        }

        public MenuItem GetMenuItemTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid())
        {
            var rootPage = GetPageTree(currentPageId, systemFilter, parentId);
            var currentCulture = _scopeService.PageContext.CurrentCulture.ToString();
            var siteRoot = _scopeService.PageContext.SiteSettingInfo.SiteRoot;

            var rootMenuItem = GetMenuItemIteratively(rootPage, currentCulture, siteRoot);
            return rootMenuItem;
        }

        private MenuItem GetMenuItemIteratively(Page page, string currentCulture, string siteRoot)
        {
            var menuItem = CreateMenuItem(page, currentCulture, siteRoot);
            menuItem.Parent = page.Parent != null ? CreateMenuItem(page.Parent, currentCulture, siteRoot) : null;

            if (page.ChildPage == null || page.ChildPage.Count <= 0) return menuItem;

            menuItem.ChildMenuItems = new List<MenuItem>();
            foreach (var child in page.ChildPage.OrderBy(p => p.PageOrder))
            {
                var childMenuItem = GetMenuItemIteratively(child, currentCulture, siteRoot);
                menuItem.ChildMenuItems.Add(childMenuItem);
            }
            return menuItem;
        }

        private MenuItem CreateMenuItem(Page page, string currentCulture, string siteRoot)
        {
            var menuItem = new MenuItem();
            var pageTranslation = page.PageTranslation.FirstOrDefault(t => t.Locale == currentCulture);

            menuItem.Page = page;
            menuItem.HasChild = page.ChildPage != null && page.ChildPage.Count > 0;
            menuItem.PageLevel = page.PageLevel != null ? (int)page.PageLevel : 0;
            menuItem.IsActive = page.IsCurrentPage;
            menuItem.IsBreadCrumb = page.IsBreadCrumb;

            if (pageTranslation == null) return menuItem;

            menuItem.PageName = pageTranslation.Name;
            menuItem.IsLinkNewWindow = pageTranslation.IsLinkNewWindow;
            if (page.PageTypeId == Globals.PageTypeStandard || page.PageTypeId == Globals.PageTypeAdmin)
            {
                menuItem.URL = siteRoot + pageTranslation.URL;
            }
            //else if (page.PageTypeId == Globals.PageTypeAdmin)
            //{
            //    menuItem.URL = $"{siteRoot}modules/{page?.AdminPage?.ModuleName}/admin/{page?.AdminPage?.EntityName}";
            //}
            else
            {
                menuItem.URL = pageTranslation.RedirectUrl;
            }

            return menuItem;
        }

        public IList<Page> GetBreadCrumbs(Guid currentPageId)
        {
            var root = _pageRepository.GetPageTree();
            FilterPage(root, currentPageId);
            breadcrumbs = new List<Page>();
            SetBreadCrumb(currentPageId, root);
            return breadcrumbs.OrderBy(p => p.PageLevel).ToList();

        }

        public Page CreatePage(Page page)
        {       //Set page URL based on the parent page
            var parentUrls = GetParentUrl(page.ParentId);
            foreach (var pageTranslation in page.PageTranslation)
            {
                var parentUrl = parentUrls[pageTranslation.Locale];
                var pageName = Regex.Replace(pageTranslation.Name, @"\s+", "");
                pageTranslation.URL = !string.IsNullOrEmpty(parentUrl)
                    ? $"{parentUrl}/{pageName}"
                    : pageName;
            }

            SetDefaultPermissions(page);

            var newPage = _pageRepository.CreatePage(page);
            return newPage;

        }

        public Page UpdatePageTree(Page page)
        {
            if (page == null) throw new InvalidOperationException($"Page cannot be null");
            var parentURLs = InitParentUrls();
            UpdatePageTreeUrl(page, parentURLs);
            var resultPage = _pageRepository.UpdatePageTree(page);
            return resultPage;
        }

        public Page UpdatePageAndChildren(Page page)
        {
            if (page == null) throw new InvalidOperationException($"Page cannot be null");
            var parentUrl = GetParentUrl(page.ParentId);

            //Updating a page have an influence on it's child pages as well. Therefore, getting all child pages and updating its url
            Page pageTree;
            if (page.ParentId != null)
            {
                pageTree = _pageRepository.GetPageTree(page.Id);
            }
            else
            {
                pageTree = _pageRepository.GetPageTree();
            }
            page.ChildPage = pageTree.ChildPage;
            UpdatePageTreeUrl(page, parentUrl);
            //SetDefaultPermissions(page);
            var resultPage = _pageRepository.UpdatePageAndPermissions(page);
            return resultPage;
        }

        public bool DeletePage(Guid pageId, bool forceDelete = false)
        {
            var page = _pageRepository.GetPage(pageId);
            if (page == null) throw new InvalidOperationException($"Page cannot be found");

            page.IsActive = false;
            var resultPage = _pageRepository.UpdatePageActiveAndLayout(page);
            return resultPage != null;
        }

        public string NavigateUrl(string pageId, string locale = null)
        {
            if (string.IsNullOrEmpty(pageId)) return string.Empty;

            var id = Guid.Parse(pageId);
            var page = _pageRepository.GetPageAndPageTranslations(id);
            return NavigateUrl(page, locale);
        }

        public string NavigateUrl(Guid pageId, string locale = null)
        {
            if (pageId == Guid.Empty) return string.Empty;
            var page = _pageRepository.GetPageAndPageTranslations(pageId);
            return NavigateUrl(page, locale);
        }

        public string NavigateUrl(Page page, string locale = null)
        {
            if (page == null) return string.Empty;

            locale ??= _scopeService.PageContext.CurrentCulture.ToString().ToLower();
            var translation = page.PageTranslation.FirstOrDefault(t => string.Equals(t.Locale, locale, StringComparison.CurrentCultureIgnoreCase));

            return translation != null ? _scopeService.PageContext.SiteRoot + translation.URL : "";
        }

        public string NavigateAbsoluteUrl(Page page, string locale = null)
        {
            var isSecureConnection = _httpContextAccessor.HttpContext.Request.IsHttps;
            var requestHost = _httpContextAccessor.HttpContext.Request.Host.Host;
            var scheme = isSecureConnection ? "https://" : "http://";
            var protocolHost = scheme + requestHost;

            var relativePath = NavigateUrl(page, locale);

            return protocolHost + relativePath;
        }
        #endregion

        #region Private Methods

        private Dictionary<string, string> InitParentUrls()
        {
            var activeLocales = _languageRepository.GetActiveLocales();
            var dictionary = activeLocales.ToDictionary(k => k, v => v.ToLower());
            if (!_languageRepository.IsMultilingual())
            {
                dictionary[dictionary.First().Key] = "";
            }
            return dictionary;
        }

        private Dictionary<string, string> CopyParentUrls(IDictionary<string, string> parentUrls)
        {
            var result = InitParentUrls();
            foreach (var kv in parentUrls)
            {
                result[kv.Key] = kv.Value;
            }
            return result;
        }

        private Page GetPageTreeFrom(Page page, Guid pageId)
        {
            Page resultPage = null;
            if (page.Id == pageId)
                resultPage = page;

            if (page.ChildPage == null) return resultPage;

            foreach (var child in page.ChildPage)
            {
                var childResult = GetPageTreeFrom(child, pageId);
                if (childResult != null)
                {
                    resultPage = childResult;
                }
            }
            return resultPage;
        }

        //private Dictionary<string, string> GetParentURL(Page page)
        //{
        //    //string url = "";
        //    var URLs = InitParentUrls();
        //    if (page != null && page.ParentId != null && page.ParentId != Guid.Empty)
        //    {
        //        Page parentPage = _pageRepository.GetPageAndPageTranslations((Guid)page.ParentId);
        //        var parentURLs = GetParentURL(parentPage);
        //        if (parentPage.PageTranslation != null && parentPage.PageTranslation.Count > 0)
        //        {
        //            foreach (var pageTranslation in parentPage.PageTranslation)
        //            {
        //                if (parentURLs.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
        //                {
        //                    //parent page has translation for current locale/culturecode
        //                    URLs[pageTranslation.Locale] = parentURLs[pageTranslation.Locale] + (!string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
        //                }
        //                else
        //                {
        //                    URLs[pageTranslation.Locale] = parentURLs[Globals.FallbackLanguage] + (!string.IsNullOrEmpty(parentURLs[Globals.FallbackLanguage]) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
        //                }
        //            }
        //        }
        //    }
        //    return URLs;
        //}

        private Dictionary<string, string> GetParentUrl(Guid? parentId)
        {
            var activeLocales = _languageRepository.GetActiveLocales();
            var dictionary = activeLocales.ToDictionary(k => k, v => "");
            var pages = _pageRepository.GetPagesAndTranslations();
            var parent = pages.FirstOrDefault(p => p.Id == parentId);
            while (parent != null)
            {
                foreach (var pageTranslation in parent.PageTranslation)
                {
                    var childUrl = dictionary[pageTranslation.Locale];
                    var pageName = Regex.Replace(pageTranslation.Name, @"\s+", "");

                    dictionary[pageTranslation.Locale] = !string.IsNullOrEmpty(childUrl) ? $"{pageName}/{childUrl}" : pageName;
                }
                parent = pages.FirstOrDefault(p => p.Id == parent.ParentId);
            }


            if (!_languageRepository.IsMultilingual()) return dictionary;


            var keys = new List<string>(dictionary.Keys);
            foreach (string key in keys)
            {
                dictionary[key] = $"{key.ToLower()}/{dictionary[key]}";
            }
            return dictionary;
        }

        //private void UpdatePageTreeURL(Page page, Dictionary<string, string> parentURLs)
        //{
        //    if (page != null)
        //    {

        //        //For new page
        //        if (page.Id == Guid.Empty)
        //        {
        //            page.LastModifiedDate = page.CreatedDate = DateTime.Now;
        //        }

        //        if (page.PageTranslation != null && page.PageTranslation.Count > 0)
        //        {
        //            string fallbackParentURL = parentURLs[Globals.FallbackLanguage];
        //            foreach (var pageTranslation in page.PageTranslation)
        //            {
        //                if (parentURLs.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentURLs[pageTranslation.Locale]))
        //                {
        //                    parentURLs[pageTranslation.Locale] += "/" + pageTranslation.Name.Replace(" ", "");
        //                    var pageUrl = parentURLs[pageTranslation.Locale];
        //                    pageTranslation.URL = getUniqueUrl(pageTranslation, pageUrl, page.Id);
        //                }
        //                else
        //                {
        //                    //Parent page is not yet translated, therefore taking parent url from fallbacklanguage

        //                    parentURLs[pageTranslation.Locale] = (!string.IsNullOrEmpty(fallbackParentURL) ? "/" : "") + pageTranslation.Name.Replace(" ", "");
        //                    var pageUrl = parentURLs[pageTranslation.Locale];
        //                    pageTranslation.URL = getUniqueUrl(pageTranslation, pageUrl, page.Id);
        //                }

        //            }
        //        }

        //        //var currentPageTranslation = GetPageTranslation(page);
        //        //if (currentPageTranslation != null)
        //        //{
        //        //    parentURL += "/" + currentPageTranslation.Name.Replace(" ", "");
        //        //    currentPageTranslation.URL = parentURL;
        //        //    //TODO: This has to be set in client side when multilingual has been implemented
        //        //    currentPageTranslation.Locale = Globals.FallbackLanguage;
        //        //}

        //        if (page.ChildPage != null && page.ChildPage.Count > 0)
        //        {
        //            //foreach (var child in page.ChildPages)
        //            //{
        //            //    ProcessPageTranslation(child, parentURL);
        //            //}

        //            foreach (var child in page.ChildPage)
        //            {
        //                //if (page.PageLevel == 0)
        //                var pUrl = CopyParentUrls(parentURLs);

        //                UpdatePageTreeURL(child, pUrl);
        //            }
        //        }
        //    }
        //}



        private void UpdatePageTreeUrl(Page page, IDictionary<string, string> parentUrls)
        {
            if (page == null) return;

            if (page.PageTranslation != null && page.PageTranslation.Count > 0)
            {
                var fallbackParentUrl = parentUrls[Globals.FallbackLanguage];
                foreach (var pageTranslation in page.PageTranslation)
                {
                    var pageName = Regex.Replace(pageTranslation.Name, @"\s+", "");
                    string pageUrl;
                    if (parentUrls.ContainsKey(pageTranslation.Locale) && !string.IsNullOrEmpty(parentUrls[pageTranslation.Locale]))
                    {
                        pageUrl = $"{parentUrls[pageTranslation.Locale]}/{pageName}";
                    }
                    else
                    {
                        //Parent page is not yet translated, therefore taking parent url from fallback language
                        pageUrl = !string.IsNullOrEmpty(fallbackParentUrl) ? $"/{pageName}" : pageName;
                    }
                    pageUrl = GetUniqueUrl(pageTranslation, pageUrl, page.Id);
                    parentUrls[pageTranslation.Locale] = pageUrl;
                    pageTranslation.URL = pageUrl;
                }
            }

            if (page.ChildPage == null || page.ChildPage.Count <= 0) return;

            foreach (var child in page.ChildPage)
            {
                var pUrl = CopyParentUrls(parentUrls);
                UpdatePageTreeUrl(child, pUrl);
            }
        }

        private string GetUniqueUrl(PageTranslation pageTranslation, string pageUrl, Guid pageId)
        {
            var duplicateTranslation = _pageRepository.GetPageTranslation(pageUrl.ToLower());

            if (duplicateTranslation == null || duplicateTranslation.PageId == pageId) return pageUrl;
            while (duplicateTranslation != null && string.Equals(duplicateTranslation.URL, pageUrl, StringComparison.CurrentCultureIgnoreCase))
            {
                pageUrl += "1";
                pageTranslation.Name += "1";
                duplicateTranslation = _pageRepository.GetPageTranslation(pageUrl);
            }
            return pageUrl;
        }

        private void FilterPage(Page page, Guid currentPageId, Func<Page, bool> predicate = null)
        {
            if (page == null) return;

            if (page.Id == currentPageId)
            {
                page.IsCurrentPage = true;
                activePage = page;
            }

            //Page filter
            if (page.ChildPage != null)
            {
                if (predicate != null)
                    page.ChildPage = page.ChildPage.Where(predicate).ToList();

                page.ChildPage = page.ChildPage.OrderBy(p => p.PageOrder).ToList();
            }

            if (page.ChildPage == null || page.ChildPage.Count <= 0) return;

            foreach (var child in page.ChildPage)
            {
                FilterPage(child, currentPageId, predicate);
            }
        }



        private bool IsCurrentHasSelected(Guid currentPageId, Page currentLevel)
        {
            var isCurrentBreadCrumb = false;
            if (currentLevel == null) return isCurrentBreadCrumb;

            var isChildActive = currentLevel?.ChildPage?.Any(child => child.Id == currentLevel.Id);
            isCurrentBreadCrumb = currentLevel.IsCurrentPage || (isChildActive ?? false);

            if (!isCurrentBreadCrumb && currentLevel.ChildPage != null && currentLevel.ChildPage.Count > 0)
            {
                foreach (var child in currentLevel.ChildPage)
                {
                    isCurrentBreadCrumb = IsCurrentHasSelected(currentPageId, child);
                    if (isCurrentBreadCrumb)
                        break;
                }
            }
            currentLevel.IsBreadCrumb = isCurrentBreadCrumb;
            return isCurrentBreadCrumb;
        }

        private void SetBreadCrumb(Guid currentPageId, Page root)
        {
            IsCurrentHasSelected(currentPageId, root);
        }
        private static void SetDefaultPermissions(Page page)
        {
            if (page.PagePermissions != null && page.PagePermissions.Count > 0)
            {
                //Remove Admin permissions, if the user set it manually
                var permissionsToRemove = page.PagePermissions.Where(p => p.RoleId == Globals.AdministratorRoleId).ToList();
                foreach (var permission in permissionsToRemove)
                {
                    page.PagePermissions.Remove(permission);
                }
            }
            else
            {
                page.PagePermissions = new List<PagePermission>();
            }

            //Always add PagePermission for Admin by default
            page.PagePermissions.Add(new PagePermission
            {
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.PageViewPermissionId,
            });

            page.PagePermissions.Add(new PagePermission
            {
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.PageEditPermissionId,
            });
        }

        #endregion
    }
}
