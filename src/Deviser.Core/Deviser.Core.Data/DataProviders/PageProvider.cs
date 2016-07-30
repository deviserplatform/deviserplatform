using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{

    public interface IPageProvider
    {
        Page GetPageTree();
        List<Page> GetPages();
        Page GetPage(Guid pageId);
        Page CreatePage(Page page);
        Page UpdatePage(Page page);
        Page UpdatePageTree(Page page);
        List<PageTranslation> GetPageTranslations(string locale);
        PageTranslation GetPageTranslation(string url);
        List<PageModule> GetPageModules(Guid pageId);
        PageModule GetPageModule(Guid pageModuleId);
        PageModule GetPageModuleByContainer(Guid containerId);
        PageModule CreatePageModule(PageModule pageModule);
        PageModule UpdatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);
        List<PagePermission> AddPagePermissions(List<PagePermission> pagePermissions);
        List<ModulePermission> AddModulePermissions(List<ModulePermission> modulePermissions);

    }

    public class PageProvider : DataProviderBase, IPageProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public PageProvider(ILifetimeScope container)
            : base(container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public Page GetPageTree()
        {
            try
            {
                /*IEnumerable<> returnData = context.Pages.Include(x => x.ChildPages
                                                                    .Select(y => y.ChildPages.Select(c => c.ChildPages.Select(gc => gc.ChildPages.Select(ggc => ggc.ChildPages)))))
                                                                    .Where(.Where(e=>e.ParentId==null&&e.IsDeleted==false));*/
                /*List<> returnData = new List<>();
                returnData.Add(context.Pages.ToList().First());*/
                using (var context = new DeviserDBContext(dbOptions))
                {
                    //return context.Page
                    //.Include(p => p.PageTranslation) //("PageTranslations")  
                    //.ToList().First();

                    //return context.Page                        
                    //    .Include(p=>p.ChildPage)
                    //    .ThenInclude(p=>p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .ThenInclude(p => p.ChildPage)
                    //    .Include(p => p.PageTranslation)
                    //    .Where(p => p.ParentId == null).First();

                    var rootOnly = context.Page
                            .Where(p => p.ParentId == null)
                            .First();
                    GetPageTree(context, rootOnly);
                    return rootOnly;
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageTree", ex);
            }
            return null;
        }

        private void GetPageTree(DeviserDBContext context, Page page)
        {
            //Page resultPage = null;

            page.ChildPage = context.Page
                .Include(p => p.PageTranslation)
                .Include(p => p.PagePermissions)
                .Where(p => p.ParentId == page.Id).ToList();

            if (page.ChildPage != null)
            {
                foreach (var child in page.ChildPage)
                {
                    GetPageTree(context, child);
                }
            }
            //return resultPage;
        }

        public List<Page> GetPages()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Page> returnData = context.Page
                                .Where(e => e.ParentId == null)
                                //.Include("PageTranslations").Include("ChildPages").Include("PageModules").Include("PageModules.Module")
                                .Include(p => p.PageTranslation).Include(p => p.ChildPage)
                                .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                                .OrderBy(p => p.Id)
                                .ToList();

                    return new List<Page>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPages", ex);
            }
            return null;
        }

        public Page GetPage(Guid pageId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Page returnData = context.Page
                               .Where(e => e.Id == pageId)
                               .Include(p => p.PageTranslation)
                               .Include(p => p.PagePermissions)
                               .Include(p => p.Layout)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.PageContentTranslation)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct => ct.ContentDataType)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct=> ct.ContentTypeProperties).ThenInclude(ctp=>ctp.Property).ThenInclude(p=>p.PropertyOptionList)
                               .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                               .OrderBy(p => p.Id)
                               .FirstOrDefault();

                    if (returnData.PageModule != null)
                    {
                        returnData.PageModule = returnData.PageModule.Where(pm => !pm.IsDeleted).ToList();
                    }

                    if (returnData.PageContent != null)
                    {
                        returnData.PageContent = returnData.PageContent.Where(pc => !pc.IsDeleted).ToList();
                    }

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }

        public Page CreatePage(Page page)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Page resultPage;
                    page.CreatedDate = DateTime.Now; page.LastModifiedDate = DateTime.Now;
                    resultPage = context.Page.Add(page).Entity;
                    context.SaveChanges();
                    return resultPage;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreatePage", ex);
            }
            return null;
        }

        /// <summary>
        /// Updates page and add/remove permissions
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePage(Page page)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Page resultPage;
                    page.LastModifiedDate = DateTime.Now;
                    //resultPage = context.UpdateGraph<Page>(page, map => map.OwnedCollection(p => p.PageTranslations));
                    resultPage = context.Page.Update(page).Entity;
                    foreach (var translation in page.PageTranslation)
                    {
                        if (context.PageTranslation.Any(pt => pt.Locale == translation.Locale && pt.PageId == translation.PageId))
                        {
                            //translation exist
                            context.PageTranslation.Update(translation);
                        }
                        else
                        {
                            context.PageTranslation.Add(translation);
                        }
                    }
                    
                    if (page.PagePermissions != null && page.PagePermissions.Count > 0)
                    {
                        //Filter deleted permissions in UI and delete all of them
                        var toDelete = context.PagePermission.Where(dbPermission => dbPermission.PageId == page.Id &&
                        !page.PagePermissions.Any(pagePermission => pagePermission.PermissionId == dbPermission.PermissionId && pagePermission.RoleId == dbPermission.RoleId)).ToList();
                        if (toDelete != null && toDelete.Count > 0)
                            context.PagePermission.RemoveRange(toDelete);

                        //Filter new permissions which are not in db and add all of them
                        var toAdd = page.PagePermissions.Where(pagePermission => !context.PagePermission.Any(dbPermission =>
                        dbPermission.PermissionId == pagePermission.PermissionId &&
                        dbPermission.PageId == pagePermission.PageId &&
                        dbPermission.RoleId == pagePermission.RoleId)).ToList();
                        if (toAdd != null && toAdd.Count > 0)
                        {
                            foreach (var permission in toAdd)
                            {
                                //permission.Page = null;
                                if (permission.Id == Guid.Empty)
                                    permission.Id = Guid.NewGuid();
                                context.PagePermission.Add(permission);
                            }
                        }
                    }


                    //context.PageTranslation.UpdateRange(page.PageTranslation);
                    context.SaveChanges();
                    return resultPage;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePage", ex);
            }
            return null;
        }

        public Page UpdatePageTree(Page page)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Page resultPage = null;
                    page.LastModifiedDate = DateTime.Now;
                    UpdatePageTreeTree(context, page);
                    context.SaveChanges();
                    resultPage = context.Page.ToList().First();
                    return resultPage;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageTree", ex);
            }
            return null;
        }

        private void UpdatePageTreeTree(DeviserDBContext context, Page page)
        {
            if (page != null && page.ChildPage != null)
            {
                //context.UpdateGraph(page, map => map.AssociatedCollection(p => p.ChildPages)
                //                                  .OwnedCollection(p => p.PageTranslations));

                context.Page.Update(page);
                context.SaveChanges();

                if (page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                        if (child != null && child.ChildPage != null)
                        {
                            if (child.ChildPage.Count > 0)
                            {
                                //foreach (var grandChild in child.ChildPages)
                                UpdatePageTreeTree(context, child);
                            }
                            else if (child.Id != Guid.Empty)
                            {
                                //context.UpdateGraph(child, map => map.AssociatedCollection(p => p.ChildPages)
                                //                  .OwnedCollection(p => p.PageTranslations));
                                context.Page.Update(page);
                                context.SaveChanges();
                            }
                        }

                }

            }
        }

        public List<PageTranslation> GetPageTranslations(string locale)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<PageTranslation> returnData = context.PageTranslation
                    .Where(e => e.Locale.ToLower() == locale.ToLower())
                    .OrderBy(p => p.PageId)
                    .ToList();
                    return new List<PageTranslation>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageTranslations", ex);
            }
            return null;
        }

        public PageTranslation GetPageTranslation(string url)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PageTranslation
                    .Where(e => e.URL.ToLower() == url.ToLower())
                    .OrderBy(p => p.PageId)
                    .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageTranslation", ex);
            }
            return null;
        }

        public List<PageModule> GetPageModules(Guid pageId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<PageModule> returnData = context.PageModule
                                .Where(e => e.PageId == pageId && !e.IsDeleted)
                                .Include(e => e.Module)
                                .Include(e => e.ModuleAction)
                                .OrderBy(p => p.Id)
                                .ToList();

                    return new List<PageModule>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageModules", ex);
            }
            return null;
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageModule returnData = context.PageModule
                               .Where(e => e.Id == pageModuleId && !e.IsDeleted)
                               .OrderBy(p => p.Id)
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }

        public PageModule GetPageModuleByContainer(Guid containerId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageModule returnData = context.PageModule
                               .Where(e => e.ContainerId == containerId && !e.IsDeleted)
                               .OrderBy(p => p.Id)
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }

        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageModule resultPageModule;
                    resultPageModule = context.PageModule.Add(pageModule).Entity;
                    context.SaveChanges();
                    return resultPageModule;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreatePageModule", ex);
            }
            return null;
        }

        public PageModule UpdatePageModule(PageModule pageModule)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageModule resultPageModule;
                    resultPageModule = context.PageModule.Update(pageModule).Entity;

                    UpdateModulePermission(pageModule, context);

                    context.SaveChanges();
                    return resultPageModule;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageModule", ex);
            }
            return null;
        }
        
        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    foreach (var pageModule in pageModules)
                    {
                        if (context.PageModule.Any(pm => pm.Id == pageModule.Id))
                        {
                            //page module exist, therefore update it
                            context.PageModule.Update(pageModule);
                            //UpdateModulePermission(pageModule, context); //Here, intensions is mostly to update the container. Moreover, permissions might not be included in each page module object.
                        }
                        else
                        {
                            context.PageModule.Add(pageModule);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageModules", ex);
            }
        }

        /// <summary>
        /// Add permissions only if its not exisit in db
        /// </summary>
        /// <param name="pagePermissions"></param>
        /// <returns></returns>
        public List<PagePermission> AddPagePermissions(List<PagePermission> pagePermissions)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {                    
                    if (pagePermissions != null && pagePermissions.Count > 0)
                    {
                        //Filter new permissions which are not in db and add all of them
                        var toAdd = pagePermissions.Where(pagePermission => !context.PagePermission.Any(dbPermission =>
                        dbPermission.PermissionId == pagePermission.PermissionId &&
                        dbPermission.PageId == pagePermission.PageId &&
                        dbPermission.RoleId == pagePermission.RoleId)).ToList();
                        if (toAdd != null && toAdd.Count > 0)
                        {
                            foreach (var permission in toAdd)
                            {
                                //permission.Page = null;
                                if (permission.Id == Guid.Empty)
                                    permission.Id = Guid.NewGuid();
                                context.PagePermission.Add(permission);
                            }
                        }

                        context.SaveChanges();
                        return toAdd;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while adding page permissions", ex);
            }
            return null;
        }

        /// <summary>
        /// Add permissions only if its not exist in db
        /// </summary>
        /// <param name="modulePermissions"></param>
        /// <returns></returns>
        public List<ModulePermission> AddModulePermissions(List<ModulePermission> modulePermissions)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    if (modulePermissions != null && modulePermissions.Count > 0)
                    {
                        //Filter new permissions which are not in db and add all of them
                        var toAdd = modulePermissions.Where(pagePermission => !context.ModulePermission.Any(dbPermission =>
                        dbPermission.PermissionId == pagePermission.PermissionId &&
                        dbPermission.PageModuleId == pagePermission.PageModuleId &&
                        dbPermission.RoleId == pagePermission.RoleId)).ToList();
                        if (toAdd != null && toAdd.Count > 0)
                        {
                            foreach (var permission in toAdd)
                            {
                                //permission.Page = null;
                                if (permission.Id == Guid.Empty)
                                    permission.Id = Guid.NewGuid();
                                context.ModulePermission.Add(permission);
                            }
                        }

                        context.SaveChanges();
                        return toAdd;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while addming module persmissions", ex);
            }
            return null;
        }

        private void UpdateModulePermission(PageModule pageModule, DeviserDBContext context)
        {
            if (pageModule.ModulePermissions != null && pageModule.ModulePermissions.Count > 0)
            {
                //Filter deleted permissions in UI and delete all of them
                var toDelete = context.ModulePermission.Where(dbPermission => dbPermission.PageModuleId == pageModule.Id &&
                !pageModule.ModulePermissions.Any(modulePermission => modulePermission.PermissionId == dbPermission.PermissionId && modulePermission.RoleId == dbPermission.RoleId)).ToList();
                if (toDelete != null && toDelete.Count > 0)
                    context.ModulePermission.RemoveRange(toDelete);

                //Filter new permissions which are not in db and add all of them
                var toAdd = pageModule.ModulePermissions.Where(modulePermission => !context.PagePermission.Any(dbPermission =>
                dbPermission.PermissionId == modulePermission.PermissionId &&
                dbPermission.PageId == modulePermission.PageModuleId &&
                dbPermission.RoleId == modulePermission.RoleId)).ToList();
                if (toAdd != null && toAdd.Count > 0)
                {
                    foreach (var permission in toAdd)
                    {
                        //permission.Page = null;
                        if (permission.Id == Guid.Empty)
                            permission.Id = Guid.NewGuid();
                        context.ModulePermission.Add(permission);
                    }
                }
            }
        }
    }
}