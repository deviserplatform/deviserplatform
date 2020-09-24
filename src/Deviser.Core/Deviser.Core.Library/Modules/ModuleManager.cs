using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Library.Modules
{
    public class ModuleManager : PageManager, IModuleManager
    {
        //Logger
        private readonly ILogger<ModuleManager> _logger;
        private readonly IScopeService _scopeService;
        public ModuleManager(ILogger<ModuleManager> logger,
            IScopeService scopeService,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _logger = logger;
            _scopeService = scopeService;
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            var pageModule = _pageRepository.GetPageModule(pageModuleId);
            if (pageModule != null)
            {
                pageModule.HasEditPermission = HasEditPermission(pageModule);
            }
            return pageModule;
        }

        public IList<PageModule> GetPageModuleByPage(Guid pageId)
        {
            var pageModules = _pageRepository.GetPageModules(pageId);
            if (pageModules == null) return pageModules;

            foreach (var pageModule in pageModules)
            {
                pageModule.HasEditPermission = HasEditPermission(pageModule);
            }
            return pageModules;
        }

        public IList<PageModule> GetDeletedPageModules()
        {
            var pageModules = _pageRepository.GetDeletedPageModules();
            return pageModules;
        }
        public PageModule CreateUpdatePageModule(PageModule pageModule)
        {
            if (pageModule == null) throw new InvalidOperationException($"Parameter PageModule cannot be null");

            var dbPageModule = _pageRepository.GetPageModule(pageModule.Id);
            if (dbPageModule == null)
            {
                dbPageModule = _pageRepository.CreatePageModule(pageModule);
                var adminPermissions = AddAdminPermissions(dbPageModule) as List<ModulePermission>;

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
                dbPageModule.IsActive = true;
                dbPageModule.ContainerId = pageModule.ContainerId;
                dbPageModule.SortOrder = pageModule.SortOrder;
                dbPageModule.Properties = pageModule.Properties;
                dbPageModule = _pageRepository.UpdatePageModule(dbPageModule);
            }

            dbPageModule.HasEditPermission = HasEditPermission(dbPageModule);

            return dbPageModule;
        }

        public void UpdatePageModules(IList<PageModule> pageModules)
        {
            if (pageModules == null) throw new InvalidOperationException($"Parameter PageModule cannot be null");

            _pageRepository.AddOrUpdatePageModules(pageModules);
        }

        public PageModule UpdateModulePermission(PageModule pageModule)
        {
            return _pageRepository.UpdateModulePermission(pageModule);
        }

        private IList<ModulePermission> AddAdminPermissions(PageModule pageModule)
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
            adminPermissions = _pageRepository.AddModulePermissions(adminPermissions) as List<ModulePermission>;
            return adminPermissions;
        }

        public bool HasViewPermission(PageModule pageModule, bool isForCurrentRequest = false)
        {
            if (pageModule == null || pageModule.ModulePermissions == null)
                throw new ArgumentNullException("pageModule.ModulePermissions", "PageModule and ModulePermissions should not be null");

            var result = (pageModule.ModulePermissions.Any(modulePermission => modulePermission.PermissionId == Globals.ModuleViewPermissionId &&
              (modulePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == modulePermission.RoleId)))));

            var page = isForCurrentRequest ? _scopeService.PageContext.CurrentPage : _pageRepository.GetPageAndPagePermissions(pageModule.PageId);
            return result || (pageModule.InheritViewPermissions && HasViewPermission(page));
        }

        public bool HasEditPermission(PageModule pageModule, bool isForCurrentRequest = false)
        {
            if (pageModule == null && pageModule.ModulePermissions == null)
                throw new ArgumentNullException("pageModule.ModulePermissions", "PageModule and ModulePermissions should not be null");

            var result = (pageModule.ModulePermissions.Any(modulePermission => modulePermission.PermissionId == Globals.ModuleEditPermissionId &&
               (modulePermission.RoleId == Globals.AllUsersRoleId || (IsUserAuthenticated && CurrentUserRoles.Any(role => role.Id == modulePermission.RoleId)))));
            var page = isForCurrentRequest ? _scopeService.PageContext.CurrentPage : _pageRepository.GetPageAndPagePermissions(pageModule.PageId);
            return result || (pageModule.InheritEditPermissions && HasEditPermission(page));
        }
    }
}
