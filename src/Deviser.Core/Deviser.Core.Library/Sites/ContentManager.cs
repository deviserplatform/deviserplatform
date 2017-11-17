using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Sites
{
    public class ContentManager : PageManager, IContentManager
    {
        //Logger
        private readonly ILogger<ContentManager> logger;

        private IPageContentProvider pageContentProvider;

        public ContentManager(ILifetimeScope container)
            : base(container)
        {
            logger = container.Resolve<ILogger<ContentManager>>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
            httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            roleProvider = container.Resolve<IRoleProvider>();
        }

        public PageContent Get(Guid pageContentId)
        {
            try
            {
                var result = pageContentProvider.Get(pageContentId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public List<PageContent> Get(Guid pageId, string cultureCode)
        {
            try
            {
                var pageContents = pageContentProvider.Get(pageId, cultureCode);
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
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public List<PageContent> Get()
        {
            try
            {
                var result = pageContentProvider.Get();
                return result;
            }
            catch (Exception ex)
            {

                logger.LogError("Error occured while getting deleted page contents", ex);
            }
            return null;

        }

        public PageContent RestorePageContent(Guid id)
        {
            try
            {
                var result = pageContentProvider.RestorePageContent(id);
                return result;
            }
            catch(Exception ex)
            {
                logger.LogError("Error occured while restoring page content", ex);
            }
            return null;
        }
        public PageContent AddOrUpdatePageContent(PageContent pageContent)
        {
            try
            {
                PageContent result = pageContentProvider.Get(pageContent.Id);
                if (result == null)
                {
                    result = pageContentProvider.Create(pageContent);

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
                    result.ContainerId = pageContent.ContainerId;
                    result.SortOrder = pageContent.SortOrder;
                    result.LastModifiedDate = DateTime.Now;
                    result.Properties = pageContent.Properties;
                    result = pageContentProvider.Update(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page content"), ex);
            }
            return null;
        }

        public void AddOrUpdatePageContents(List<PageContent> contents)
        {
            try
            {
                if (contents != null)
                {
                    pageContentProvider.AddOrUpdate(contents);

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
                logger.LogError(string.Format("Error occured while updating page content"), ex);
            }
        }

        public bool RemovePageContent(Guid id)
        {
            var content = pageContentProvider.Get(id);
            if (content != null)
            {
                content.IsDeleted = true;
                pageContentProvider.Update(content);
                return true;
            }
            return false;
        }

        public bool DeletePageContent(Guid id)
        {
            bool result = pageContentProvider.DeletePageContent(id);
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
                    pageContentProvider.UpdateContentPermission(pageContent);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page content permissions"), ex);
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
            adminPermissions = pageContentProvider.AddContentPermissions(adminPermissions);
            return adminPermissions;
        }

        public bool HasViewPermission(PageContent pageContent)
        {
            if (pageContent == null && pageContent.ContentPermissions == null)
                throw new ArgumentNullException("pageContent.ContentPermissions", "PageContent and ContentPermissions should not be null");

            var result = (pageContent.ContentPermissions.Any(contentPermission => contentPermission.PermissionId == Globals.ContentViewPermissionId &&
          (contentPermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == contentPermission.RoleId)))));
            var page = pageProvider.GetPage(pageContent.PageId);
            return result || (pageContent.InheritViewPermissions && HasViewPermission(page));
        }

        public bool HasEditPermission(PageContent pageContent)
        {
            if (pageContent == null && pageContent.ContentPermissions == null)
                throw new ArgumentNullException("pageContent.ContentPermissions", "PageContent and ContentPermissions should not be null");

            var result = (pageContent.ContentPermissions.Any(contentPermission => contentPermission.PermissionId == Globals.ContentEditPermissionId &&
           (contentPermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == contentPermission.RoleId)))));
            var page = pageProvider.GetPage(pageContent.PageId);
            return result || (pageContent.InheritEditPermissions && HasViewPermission(page));
        }
    }
}
