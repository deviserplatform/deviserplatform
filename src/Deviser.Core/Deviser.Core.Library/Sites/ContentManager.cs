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
            try
            {
                var result = _pageContentRepository.Get(pageContentId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public List<PageContent> Get(Guid pageId, string cultureCode)
        {
            try
            {
                var pageContents = _pageContentRepository.Get(pageId, cultureCode);
                if (pageContents != null)
                {
                    foreach (var pageContent in pageContents)
                    {
                        pageContent.HasEditPermission = HasEditPermission(pageContent);
                    }
                }
                return pageContents;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public List<PageContent> Get()
        {
            try
            {
                var result = _pageContentRepository.Get();
                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError("Error occured while getting deleted page contents", ex);
            }
            return null;

        }

        public PageContent RestorePageContent(Guid id)
        {
            try
            {
                var result = _pageContentRepository.RestorePageContent(id);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured while restoring page content", ex);
            }
            return null;
        }
        public PageContent AddOrUpdatePageContent(PageContent pageContent)
        {
            try
            {
                PageContent result = _pageContentRepository.Get(pageContent.Id);
                if (result == null)
                {
                    result = _pageContentRepository.Create(pageContent);

                    List<ContentPermission> adminPermissions = AddAdminPermissions(result);

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
                    pageContent.IsDeleted = false;
                    result.Title = pageContent.Title;
                    result.ContainerId = pageContent.ContainerId;
                    result.SortOrder = pageContent.SortOrder;
                    result.LastModifiedDate = DateTime.Now;
                    result.Properties = pageContent.Properties;
                    result = _pageContentRepository.Update(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page content"), ex);
            }
            return null;
        }

        public void AddOrUpdatePageContents(List<PageContent> contents)
        {
            try
            {
                if (contents != null)
                {
                    _pageContentRepository.AddOrUpdate(contents);

                    foreach (var pageContent in contents)
                    {
                        var adminPermissions = AddAdminPermissions(pageContent);

                        if (pageContent.ContentPermissions == null)
                        {
                            pageContent.ContentPermissions = adminPermissions;
                        }
                        else
                        {
                            adminPermissions.AddRange(pageContent.ContentPermissions);
                            pageContent.ContentPermissions = adminPermissions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content"), ex);
            }
        }

        public bool RemovePageContent(Guid id)
        {
            var content = _pageContentRepository.Get(id);
            if (content != null)
            {
                content.IsDeleted = true;
                _pageContentRepository.Update(content);
                return true;
            }
            return false;
        }

        public bool DeletePageContent(Guid id)
        {
            bool result = _pageContentRepository.DeletePageContent(id);
            if (result)
                return true;

            return false;
        }

        public void UpdateContentPermission(PageContent pageContent)
        {
            try
            {
                if (pageContent != null)
                {
                    _pageContentRepository.UpdateContentPermission(pageContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content permissions"), ex);
            }
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
