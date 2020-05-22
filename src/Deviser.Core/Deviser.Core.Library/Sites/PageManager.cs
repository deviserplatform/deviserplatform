using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Sites
{
    public class PageManager : IPageManager
    {
        //Logger
        private readonly ILogger<PageManager> _logger;

        protected readonly IServiceProvider _serviceProvider;
        protected readonly IPageRepository _pageRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IRoleRepository _roleRepository;

        private bool _isAuthenticated;
        private string _currentUserName;
        private List<Role> _currentUserRoles;

        public PageManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<PageManager>>();
            _pageRepository = serviceProvider.GetService<IPageRepository>();
            _roleRepository = serviceProvider.GetService<IRoleRepository>();
            _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        }

        protected bool IsUserAuthenticated
        {
            get
            {
                if(!_isAuthenticated)
                    _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                return _isAuthenticated; 
            }
        }

        protected string CurrentUserName
        {
            get
            {
                if(string.IsNullOrEmpty(_currentUserName))
                    _currentUserName = (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) ? _httpContextAccessor.HttpContext.User.Identity.Name : "";

                return _currentUserName;
            }
        }

        protected List<Role> CurrentUserRoles
        {
            get
            {
                if(_currentUserRoles==null)
                    _currentUserRoles = _roleRepository.GetRoles(CurrentUserName);

                return _currentUserRoles;
            }
        }

        public Page GetPageAndDependencies(Guid pageId)
        {
            try
            {
                var returnData = _pageRepository.GetPageAndDependencies(pageId);
                return returnData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }

        public Page GetPageAndTranslation(Guid pageId)
        {
            var resultPage = _pageRepository.GetPageAndPageTranslations(pageId);
            return resultPage;
        }

        public Page GetPageAndTranslationByUrl(string url, string locale)
        {
            Page resultPage = null;
            if (!string.IsNullOrEmpty(url))
            {
                var pageTranslation = _pageRepository.GetPageTranslations(locale);
                var currentPageTranslation = pageTranslation.FirstOrDefault(p => (p != null && string.Equals(p.URL, url, StringComparison.InvariantCultureIgnoreCase)));
                if (currentPageTranslation != null)
                {
                    resultPage = _pageRepository.GetPageAndPageTranslations(currentPageTranslation.PageId);
                }
            }
            return resultPage;
        }

        public bool HasViewPermission(Page page)
        {
            if (page != null && page.PagePermissions != null)
            {
                return (page.PagePermissions.Any(pagePermission => pagePermission.PermissionId == Globals.PageViewPermissionId &&
               (pagePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == pagePermission.RoleId)))));
            }
            return false;
        }

        public bool HasEditPermission(Page page)
        {
            if (page != null && page.PagePermissions != null)
            {
                return (page.PagePermissions.Any(pagePermission => pagePermission.PermissionId == Globals.PageEditPermissionId &&
               (pagePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == pagePermission.RoleId)))));
            }
            return false;
        }
    }
}
