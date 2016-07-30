using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Modules
{
    public class ModuleManager : IModuleManager
    {
        //Logger
        private readonly ILogger<ModuleManager> logger;

        IPageProvider pageProvider;
        IPageContentProvider pageContentProvider;
        ILifetimeScope container;

        public ModuleManager(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<ModuleManager>>();
            pageProvider = container.Resolve<IPageProvider>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
        }

        public List<PageModule> GetPageModuleByPage(Guid pageId)
        {
            try
            {
                var pageModules = pageProvider.GetPageModules(pageId);
                return pageModules;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
            }
            
            return null;
        }

        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    PageModule result = pageProvider.GetPageModule(pageModule.Id);                    
                    if (result == null)
                    {
                        result = pageProvider.CreatePageModule(pageModule);
                        List<ModulePermission> adminPermissions = AddAdminPermissions(result);

                        if (result.ModulePermissions == null)
                        {
                            result.ModulePermissions = adminPermissions;
                        }
                        else
                        {
                            adminPermissions.AddRange(result.ModulePermissions);
                            result.ModulePermissions = adminPermissions;
                        }
                    }

                    else 
                    {
                        result.IsDeleted = false;
                        result.ContainerId = pageModule.ContainerId;
                        result.SortOrder = pageModule.SortOrder;                        
                        result = pageProvider.UpdatePageModule(result);
                    }                        
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
            }
            return null;
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
            adminPermissions = pageProvider.AddModulePermissions(adminPermissions);
            return adminPermissions;
        }

        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                if (pageModules != null)
                {
                    pageProvider.UpdatePageModules(pageModules);

                    foreach(var pageModule in pageModules)
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
                logger.LogError(string.Format("Error occured while updating pageModules"), ex);
            }
        }

        public PageContent CreatePageContent(PageContent pageContent)
        {
            try
            {
                PageContent result = pageContentProvider.Get(pageContent.Id);
                if (result == null)
                    result = pageContentProvider.Create(pageContent);
                else
                {                    
                    pageContent.IsDeleted = false;
                    result.ContainerId = pageContent.ContainerId;
                    result.SortOrder = pageContent.SortOrder;
                    result.LastModifiedDate = DateTime.Now;
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
    }
}
