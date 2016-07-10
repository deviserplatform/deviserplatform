using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Sites
{
    public class PageManager : IPageManager
    {
        private ILifetimeScope container;
        private IPageProvider pageProvider;
        private IHttpContextAccessor httpContextAccessor;
        private IRoleProvider roleProvider;

        public PageManager(ILifetimeScope container)
        {
            this.container = container;
            pageProvider = container.Resolve<IPageProvider>();
            roleProvider = container.Resolve<IRoleProvider>();
            httpContextAccessor = container.Resolve<IHttpContextAccessor>();
        }

        private bool IsUserAuthenticated
        {
            get
            {
                return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        private string CurrentUserName
        {
            get
            {
                return (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) ? httpContextAccessor.HttpContext.User.Identity.Name : "";
            }
        }

        private List<Role> CurrentUserRoles
        {
            get
            {
                return roleProvider.GetRoles(CurrentUserName);
            }
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
        
        public bool IsPageAccessible(Page page)
        {
            if (page != null && page.PagePermissions != null)
            {
                return (page.PagePermissions.Any(pagePermission => pagePermission.PermissionId == Globals.PageViewPermissionId &&
               (pagePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == pagePermission.RoleId)))));
            }
            return false;
        }
    }
}
