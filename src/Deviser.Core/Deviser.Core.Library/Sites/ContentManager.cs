using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Sites
{
    public class ContentManager : PageManager, IContentManager
    {
        //Logger
        private readonly ILogger<ContentManager> _logger;
        private readonly IScopeService _scopeService;
        private readonly IPageContentRepository _pageContentRepository;


        public ContentManager(ILogger<ContentManager> logger,
            IPageContentRepository pageContentRepository,
            IScopeService scopeService,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _logger = logger;
            _pageContentRepository = pageContentRepository;
            _scopeService = scopeService;
            //_httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            //_roleRepository = container.Resolve<IRoleRepository>();
        }

        public PageContent Get(Guid pageContentId)
        {
            var result = _pageContentRepository.Get(pageContentId);
            return result;
        }

        public List<PageContent> Get(Guid pageId, string cultureCode)
        {
            var pageContents = _pageContentRepository.Get(pageId, cultureCode);
            if (pageContents == null) return pageContents;
            foreach (var pageContent in pageContents)
            {
                pageContent.HasEditPermission = HasEditPermission(pageContent);
            }
            return pageContents;
        }

        public List<PageContent> GetDeletedPageContents()
        {
            var result = _pageContentRepository.GetDeletedPageContents();
            return result;
        }

        public PageContent RestorePageContent(Guid id)
        {
            var result = _pageContentRepository.RestorePageContent(id);
            return result;
        }
        public PageContent AddOrUpdatePageContent(PageContent pageContent)
        {
            var result = _pageContentRepository.Get(pageContent.Id);
            if (result == null)
            {
                result = _pageContentRepository.Create(pageContent);

                var adminPermissions = AddAdminPermissions(result);

                if (result.ContentPermissions == null)
                {
                    result.ContentPermissions = adminPermissions;
                }
                else
                {
                    adminPermissions.AddRange(result.ContentPermissions);
                    result.ContentPermissions = adminPermissions;
                }
            }
            else
            {
                pageContent.IsActive = true;
                result.Title = pageContent.Title;
                result.ContainerId = pageContent.ContainerId;
                result.SortOrder = pageContent.SortOrder;
                result.LastModifiedDate = DateTime.Now;
                result.Properties = pageContent.Properties;
                result = _pageContentRepository.Update(result);
            }
            return result;
        }

        public void AddOrUpdatePageContents(List<PageContent> contents)
        {
            if (contents == null) throw new InvalidOperationException($"PageContents cannot be null");

            _pageContentRepository.AddOrUpdate(contents);
        }

        public bool RemovePageContent(Guid id)
        {
            var content = _pageContentRepository.Get(id);
            if (content == null) throw new InvalidOperationException($"PageContents cannot be found {id}");
            content.IsActive = false;
            _pageContentRepository.Update(content);
            return true;
        }

        public bool DeletePageContent(Guid id)
        {
            var result = _pageContentRepository.DeletePageContent(id);
            return result;
        }

        public PageContent UpdateContentPermission(PageContent pageContent)
        {
            return _pageContentRepository.UpdateContentPermission(pageContent);
        }

        private List<ContentPermission> AddAdminPermissions(PageContent pageContent)
        {
            //Update admin permissions
            var adminPermissions = new List<ContentPermission>();
            adminPermissions.Add(new ContentPermission
            {
                PageContentId = pageContent.Id,
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.ContentViewPermissionId,
            });

            adminPermissions.Add(new ContentPermission
            {
                PageContentId = pageContent.Id,
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.ContentEditPermissionId,
            });
            adminPermissions = _pageContentRepository.AddContentPermissions(adminPermissions);
            return adminPermissions;
        }

        public bool HasViewPermission(PageContent pageContent, bool isForCurrentRequest = false)
        {
            if (pageContent == null && pageContent.ContentPermissions == null)
                throw new ArgumentNullException("pageContent.ContentPermissions", "PageContent and ContentPermissions should not be null");

            var result = (pageContent.ContentPermissions.Any(contentPermission => contentPermission.PermissionId == Globals.ContentViewPermissionId &&
          (contentPermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == contentPermission.RoleId)))));

            var page = isForCurrentRequest ? _scopeService.PageContext.CurrentPage : _pageRepository.GetPageAndPagePermissions(pageContent.PageId);
            return result || (pageContent.InheritViewPermissions && HasViewPermission(page));
        }

        public bool HasEditPermission(PageContent pageContent, bool isForCurrentRequest = false)
        {
            if (pageContent == null && pageContent.ContentPermissions == null)
                throw new ArgumentNullException("pageContent.ContentPermissions", "PageContent and ContentPermissions should not be null");

            var result = (pageContent.ContentPermissions.Any(contentPermission => contentPermission.PermissionId == Globals.ContentEditPermissionId &&
           (contentPermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == contentPermission.RoleId)))));

            var page = isForCurrentRequest ? _scopeService.PageContext.CurrentPage : _pageRepository.GetPageAndPagePermissions(pageContent.PageId);
            return result || (pageContent.InheritEditPermissions && HasEditPermission(page));
        }
    }
}
