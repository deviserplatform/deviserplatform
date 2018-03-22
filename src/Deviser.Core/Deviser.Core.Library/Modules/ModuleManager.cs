using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Modules
{
    public class ModuleManager : PageManager, IModuleManager
    {
        //Logger
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<ModuleManager>>();
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            try
            {
                var pageModule = _pageRepository.GetPageModule(pageModuleId);
                if (pageModule != null)
                {
                    pageModule.HasEditPermission = HasEditPermission(pageModule);
                }
                return pageModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting pageModule, pageModuleId: ", pageModuleId), ex);
            }

            return null;
        }

        public List<PageModule> GetPageModuleByPage(Guid pageId)
        {
            try
            {
                var pageModules = _pageRepository.GetPageModules(pageId);
                if (pageModules != null)
                {
                    foreach (var pageModule in pageModules)
                    {
                        pageModule.HasEditPermission = HasEditPermission(pageModule);
                    }
                }
                return pageModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
            }

            return null;
        }

        public List<PageModule> GetDeletedPageModules()
        {
            try
            {
                var pageModules = _pageRepository.GetDeletedPageModules();               
                return pageModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting deleted pageModules"), ex);
            }

            return null;

        }
        public PageModule CreateUpdatePageModule(PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    PageModule dbPageModule = _pageRepository.GetPageModule(pageModule.Id);
                    if (dbPageModule == null)
                    {
                        dbPageModule = _pageRepository.CreatePageModule(pageModule);
                        List<ModulePermission> adminPermissions = AddAdminPermissions(dbPageModule);

                        if (dbPageModule.ModulePermissions == null)
                        {
                            dbPageModule.ModulePermissions = adminPermissions;
                        }
                        else
                        {
                            adminPermissions.AddRange(dbPageModule.ModulePermissions);
                            dbPageModule.ModulePermissions = adminPermissions;
                        }
                    }

                    else
                    {
                        dbPageModule.Title = pageModule.Title;
                        dbPageModule.IsDeleted = false;
                        dbPageModule.ContainerId = pageModule.ContainerId;
                        dbPageModule.SortOrder = pageModule.SortOrder;                       
                        dbPageModule.Properties = pageModule.Properties;
                        dbPageModule = _pageRepository.UpdatePageModule(dbPageModule);
                    }

                    dbPageModule.HasEditPermission = HasEditPermission(dbPageModule);

                    return dbPageModule;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
            }
            return null;
        }

        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                if (pageModules != null)
                {
                    _pageRepository.UpdatePageModules(pageModules);

                    foreach (var pageModule in pageModules)
                    {
                        var adminPermissions = AddAdminPermissions(pageModule);

                        if (pageModule.ModulePermissions == null)
                        {
                            pageModule.ModulePermissions = adminPermissions;
                        }
                        else
                        {
                            adminPermissions.AddRange(pageModule.ModulePermissions);
                            pageModule.ModulePermissions = adminPermissions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating pageModules"), ex);
            }
        }

        public void UpdateModulePermission(PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    _pageRepository.UpdateModulePermission(pageModule);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page content"), ex);
            }
        }

        private List<ModulePermission> AddAdminPermissions(PageModule pageModule)
        {
            //Update admin permissions
            var adminPermissions = new List<ModulePermission>();
            adminPermissions.Add(new ModulePermission
            {
                PageModuleId = pageModule.Id,
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.ModuleViewPermissionId,
            });

            adminPermissions.Add(new ModulePermission
            {
                PageModuleId = pageModule.Id,
                RoleId = Globals.AdministratorRoleId,
                PermissionId = Globals.ModuleEditPermissionId,
            });
            adminPermissions = _pageRepository.AddModulePermissions(adminPermissions);
            return adminPermissions;
        }

        public bool HasViewPermission(PageModule pageModule)
        {
            if (pageModule == null || pageModule.ModulePermissions == null)
                throw new ArgumentNullException("pageModule.ModulePermissions", "PageModule and ModulePermissions should not be null");

            var result = (pageModule.ModulePermissions.Any(modulePermission => modulePermission.PermissionId == Globals.ModuleViewPermissionId &&
              (modulePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == modulePermission.RoleId)))));
            var page = _pageRepository.GetPage(pageModule.PageId);
            return result || (pageModule.InheritViewPermissions && HasViewPermission(page));
        }

        public bool HasEditPermission(PageModule pageModule)
        {
            if (pageModule == null && pageModule.ModulePermissions == null)
                throw new ArgumentNullException("pageModule.ModulePermissions", "PageModule and ModulePermissions should not be null");

            var result = (pageModule.ModulePermissions.Any(modulePermission => modulePermission.PermissionId == Globals.ModuleEditPermissionId &&
               (modulePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == modulePermission.RoleId)))));
            var page = _pageRepository.GetPage(pageModule.PageId);
            return result || (pageModule.InheritEditPermissions && HasEditPermission(page));
        }
    }
}
