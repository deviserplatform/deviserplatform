using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{

    public interface IPageProvider : IDataProviderBase
    {
        Page GetPageTree();
        List<Page> GetPages();
        List<Page> GetDeletedPages();
        Page GetPage(Guid pageId);
        Page CreatePage(Page dbPage);
        Page UpdatePage(Page dbPage);
        Page UpdatePageTree(Page dbPage);
        Page RestorePage(Guid id);
        List<PageTranslation> GetPageTranslations(string locale);
        PageTranslation GetPageTranslation(string url);
        List<PageModule> GetPageModules(Guid pageId);
        List<PageModule> GetDeletedPageModules();
        PageModule GetPageModule(Guid pageModuleId);
        //PageModule GetPageModuleByContainer(Guid containerId);
        PageModule CreatePageModule(PageModule dbPageModule);
        PageModule UpdatePageModule(PageModule dbPageModule);
        void UpdatePageModules(List<PageModule> dbPageModules);
        void UpdateModulePermission(PageModule dbPageModule);
        List<PagePermission> AddPagePermissions(List<PagePermission> dbPagePermissions);
        List<ModulePermission> AddModulePermissions(List<ModulePermission> dbModulePermissions);
        PageModule RestorePageModule(Guid id);
        bool DeletePageModule(Guid id);
        bool DeletePage(Guid id);
        bool DraftPage(Guid id);
        bool PublishPage(Guid id);
    }

    public class PageProvider : DataProviderBase, IPageProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> _logger;

        //Constructor
        public PageProvider(ILifetimeScope container)
            : base(container)
        {
            Container = container;
            _logger = container.Resolve<ILogger<LayoutProvider>>();
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
                using (var context = new DeviserDbContext(DbOptions))
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
                            .First(p => p.ParentId == null);
                    GetPageTree(context, rootOnly);
                    return Mapper.Map<Page>(rootOnly);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting page tree", ex);
            }
            return null;
        }

        private void GetPageTree(DeviserDbContext context, Entities.Page page)
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
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Page
                                .Where(e => e.ParentId != null)
                                //.Include("PageTranslations").Include("ChildPages").Include("PageModules").Include("PageModules.Module")
                                .Include(p => p.PageTranslation)
                                .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                                .OrderBy(p => p.PageOrder)
                                .ToList();

                    return Mapper.Map<List<Page>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all pages", ex);
            }
            return null;
        }

        public List<Page> GetDeletedPages()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Page
                                .Include(p => p.PageTranslation)
                                .Where(e => e.ParentId != null && e.IsDeleted)                                
                                .ToList();
                   
                    return Mapper.Map<List<Page>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting deleted pages", ex);
            }
            return null;
        }

        public Page GetPage(Guid pageId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Page
                               .Where(e => e.Id == pageId)
                               .Include(p => p.PageTranslation)
                               .Include(p => p.PagePermissions)
                               .Include(p => p.Layout)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.PageContentTranslation)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct => ct.ContentDataType)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct => ct.ContentTypeProperties).ThenInclude(ctp => ctp.Property).ThenInclude(p => p.OptionList)
                               .Include(p => p.PageContent).ThenInclude(pc => pc.ContentPermissions)
                               .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                               .Include(p => p.PageModule).ThenInclude(pm => pm.ModulePermissions)
                               .OrderBy(p => p.Id)
                               .FirstOrDefault();

                    if (result.PageModule != null)
                    {
                        result.PageModule = result.PageModule.Where(pm => !pm.IsDeleted).ToList();
                    }

                    if (result.PageContent != null)
                    {
                        result.PageContent = result.PageContent.Where(pc => !pc.IsDeleted).ToList();
                    }                 

                    return Mapper.Map<Page>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }

        public Page CreatePage(Page page)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPage = Mapper.Map<Entities.Page>(page);
                    dbPage.CreatedDate = DateTime.Now; dbPage.LastModifiedDate = DateTime.Now;
                    var result = context.Page.Add(dbPage).Entity;
                    context.SaveChanges();
                    return Mapper.Map<Page>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling CreatePage", ex);
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
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPage = Mapper.Map<Entities.Page>(page);

                    dbPage.LastModifiedDate = DateTime.Now;

                    var pagePermissions = dbPage.PagePermissions;
                    var pageTranslation = dbPage.PageTranslation;
                    dbPage.PagePermissions = null;
                    dbPage.PageTranslation = null;

                    var result = context.Page.Update(dbPage).Entity;
                    foreach (var translation in pageTranslation)
                    {
                        if (context.PageTranslation.Any(pt => pt.Locale == translation.Locale && pt.PageId == translation.PageId))
                        {
                            //translation.URL = GetUniqueUrl(context, translation.URL, translation.Locale);
                            //translation exist
                            translation.URL = translation.URL.ToLower();
                            context.PageTranslation.Update(translation);
                        }
                        else
                        {
                            //translation.URL = GetUniqueUrl(context, translation.URL, translation.Locale);
                            translation.PageId = dbPage.Id;
                            translation.URL = translation.URL.ToLower();
                            context.PageTranslation.Add(translation);
                        }
                    }

                    if (pagePermissions != null && pagePermissions.Count > 0)
                    {
                        //Filter deleted permissions in UI and delete all of them
                        var toDelete = context.PagePermission.Where(dbPermission => dbPermission.PageId == dbPage.Id &&
                        !pagePermissions.Any(pagePermission => pagePermission.PermissionId == dbPermission.PermissionId && pagePermission.RoleId == dbPermission.RoleId)).ToList();
                        if (toDelete != null && toDelete.Count > 0)
                            context.PagePermission.RemoveRange(toDelete);

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
                    }


                    //context.PageTranslation.UpdateRange(page.PageTranslation);
                    context.SaveChanges();
                    return Mapper.Map<Page>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePage", ex);
            }
            return null;
        }

        //private string GetUniqueUrl(DeviserDBContext context, string url, string locale)
        //{
        //    var counter = 0;
        //    while (context.PageTranslation.Any(pt => pt.Locale == locale && pt.URL == url))
        //    {
        //        counter++;
        //        url += (counter).ToString();
        //    }
        //    return url;
        //}

        public Page UpdatePageTree(Page page)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPage = Mapper.Map<Entities.Page>(page);
                    dbPage.LastModifiedDate = DateTime.Now;
                    UpdatePageTreeTree(context, dbPage);
                    context.SaveChanges();
                    var result = context.Page.ToList().First();
                    return Mapper.Map<Page>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePageTree", ex);
            }
            return null;
        }

        private void UpdatePageTreeTree(DeviserDbContext context, Entities.Page page)
        {
            if (page != null && page.ChildPage != null)
            {
                context.Page.Update(page);
                context.SaveChanges();

                if (page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                        if (child != null && child.ChildPage != null)
                        {
                            if (child.ChildPage.Count > 0)
                            {
                                UpdatePageTreeTree(context, child);
                            }
                            else if (child.Id != Guid.Empty)
                            {
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
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PageTranslation
                    .Where(e => e.Locale.ToLower() == locale.ToLower())
                    .OrderBy(p => p.PageId)
                    .ToList();
                    return Mapper.Map<List<PageTranslation>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTranslations", ex);
            }
            return null;
        }

        public PageTranslation GetPageTranslation(string url)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PageTranslation
                    .Where(e => string.Equals(e.URL, url, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(p => p.PageId)
                    .FirstOrDefault();
                    return Mapper.Map<PageTranslation>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTranslation", ex);
            }
            return null;
        }

        public List<PageModule> GetPageModules(Guid pageId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PageModule
                                .Where(e => e.PageId == pageId && !e.IsDeleted)
                                .Include(e => e.Module)
                                .Include(e => e.ModuleAction)
                                .Include(e => e.ModulePermissions)
                                .OrderBy(p => p.Id)
                                .ToList();

                    return Mapper.Map<List<PageModule>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageModules", ex);
            }
            return null;
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PageModule
                        .Include(pm => pm.ModulePermissions)
                        .Where(e => e.Id == pageModuleId && !e.IsDeleted)
                        .OrderBy(p => p.Id)
                        .FirstOrDefault();

                    return Mapper.Map<PageModule>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }

        public List<PageModule>  GetDeletedPageModules()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PageModule                               
                               .Include(P => P.Page).ThenInclude(P => P.PageTranslation)
                               .Where(e => e.IsDeleted)
                               .OrderBy(p => p.Id)
                               .ToList();

                    return Mapper.Map<List<PageModule>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageModules", ex);
            }
            return null;
        }

        //public PageModule GetPageModuleByContainer(Guid containerId)
        //{
        //    try
        //    {
        //        using (var context = new DeviserDBContext(dbOptions))
        //        {
        //            PageModule returnData = context.PageModule
        //                       .Where(e => e.ContainerId == containerId && !e.IsDeleted)
        //                       .OrderBy(p => p.Id)
        //                       .FirstOrDefault();

        //            return returnData;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("Error occured while calling GetPageModule", ex);
        //    }
        //    return null;
        //}

        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPageModule = Mapper.Map<Entities.PageModule>(pageModule);
                    var result = context.PageModule.Add(dbPageModule).Entity;
                    context.SaveChanges();
                    return Mapper.Map<PageModule>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling CreatePageModule", ex);
            }
            return null;
        }

        public PageModule UpdatePageModule(PageModule pageModule)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPageModule = Mapper.Map<Entities.PageModule>(pageModule);
                    var result = context.PageModule.Update(dbPageModule).Entity;
                    context.SaveChanges();
                    return Mapper.Map<PageModule>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePageModule", ex);
            }
            return null;
        }

        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPageModules = Mapper.Map<List<Entities.PageModule>>(pageModules);
                    foreach (var pageModule in dbPageModules)
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
                _logger.LogError("Error occured while calling UpdatePageModules", ex);
                throw;
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
                using (var context = new DeviserDbContext(DbOptions))
                {
                    if (pagePermissions != null && pagePermissions.Count > 0)
                    {
                        var dbPagePermissions = Mapper.Map<List<Entities.PagePermission>>(pagePermissions);
                        //Filter new permissions which are not in db and add all of them
                        var toAdd = dbPagePermissions.Where(pagePermission => !context.PagePermission.Any(dbPermission =>
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
                        return Mapper.Map<List<PagePermission>>(toAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while adding page permissions", ex);
            }
            return null;
        }

        /// <summary>
        /// Add permissions only if its not exist in db
        /// </summary>
        /// <param name="pagePermissions"></param>
        /// <returns></returns>
        public List<ModulePermission> AddModulePermissions(List<ModulePermission> pagePermissions)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    if (pagePermissions != null && pagePermissions.Count > 0)
                    {
                        var dbModulePermissions = Mapper.Map<List<Entities.ModulePermission>>(pagePermissions);
                        //Filter new permissions which are not in db and add all of them
                        var toAdd = dbModulePermissions.Where(modulePermission => !context.ModulePermission.Any(dbPermission =>
                        dbPermission.PermissionId == modulePermission.PermissionId &&
                        dbPermission.PageModuleId == modulePermission.PageModuleId &&
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

                        context.SaveChanges();
                        return Mapper.Map<List<ModulePermission>>(toAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while addming module persmissions", ex);
            }
            return null;
        }

        public void UpdateModulePermission(PageModule pageModule)
        {
            if (pageModule.ModulePermissions != null && pageModule.ModulePermissions.Count > 0)
            {
                var dbPageModule = Mapper.Map<Entities.PageModule>(pageModule);
                //Assuming all permissions have same pageModuleId
                var pageModuleId = dbPageModule.Id;
                var modulePermissions = dbPageModule.ModulePermissions;

                try
                {
                    using (var context = new DeviserDbContext(DbOptions))
                    {
                        //Update InheritViewPermissions only
                        var dbPageContent = context.PageModule.First(pc => pc.Id == pageModuleId);
                        dbPageContent.InheritViewPermissions = dbPageModule.InheritViewPermissions;
                        dbPageContent.InheritEditPermissions = dbPageModule.InheritEditPermissions;

                        //Filter deleted permissions in UI and delete all of them
                        var toDelete = context.ModulePermission.Where(dbPermission => dbPermission.PageModuleId == pageModuleId &&
                        !modulePermissions.Any(modulePermission => modulePermission.PermissionId == dbPermission.PermissionId && modulePermission.RoleId == dbPermission.RoleId)).ToList();
                        if (toDelete != null && toDelete.Count > 0)
                            context.ModulePermission.RemoveRange(toDelete);

                        //Filter new permissions which are not in db and add all of them
                        var toAdd = modulePermissions.Where(modulePermission => !context.ModulePermission.Any(dbPermission =>
                        dbPermission.PermissionId == modulePermission.PermissionId &&
                        dbPermission.PageModuleId == modulePermission.PageModuleId &&
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

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occured while updating module persmissions", ex);
                    throw;
                }
            }
        }

        public PageModule RestorePageModule(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbpageModule = GetDeletedPageModule(id);

                    if(dbpageModule != null)
                    {                       
                        dbpageModule.IsDeleted = false;
                        var result = context.PageModule.Update(dbpageModule).Entity;
                        context.SaveChanges();
                        return Mapper.Map<PageModule>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while restoring page module", ex);
            }
            return null;
        }

        public bool DeletePageModule(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbpageModule = GetDeletedPageModule(id);

                    if (dbpageModule != null)
                    {                       
                        context.PageModule.Remove(dbpageModule);
                        var pageModulePermissions = context.ModulePermission
                           .Where(p => p.PageModuleId == id)
                           .ToList();
                        context.ModulePermission.RemoveRange(pageModulePermissions);
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting page module", ex);
            }

            return false;
        }

        private Entities.PageModule GetDeletedPageModule(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var pageModule = context.PageModule
                        .Where(p => p.Id == id && p.IsDeleted).First();

                    return pageModule;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while accessing deleted page module", ex);
            }
            return null;
        }

        public Page RestorePage(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbpage = GetDeletedPage(id);

                    if (dbpage != null)
                    {                      
                        dbpage.IsDeleted = false;
                        var result = context.Update(dbpage).Entity;
                        context.SaveChanges();
                        return Mapper.Map<Page>(result);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTranslations", ex);
            }
            return null;
        }

        public bool DeletePage(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    //Page
                    var dbpage = GetDeletedPage(id);
                    if(dbpage != null)
                    {
                        context.Page.Remove(dbpage);
                    }

                    //PageTranslation
                    var dbpageTranslation = context.PageTranslation
                                        .Where(p => p.PageId == id)
                                        .ToList();
                    if(dbpageTranslation != null)
                    {
                        context.PageTranslation.RemoveRange(dbpageTranslation);
                    }

                    //PagePermission
                    var dbPagePermission = context.PagePermission
                        .Where(p => p.PageId == id)
                        .ToList();
                    if(dbPagePermission != null)
                    {
                        context.PagePermission.RemoveRange(dbPagePermission);
                    }

                    //PageContent
                    var dbpagecontent = context.PageContent
                                    .Where(p => p.PageId == id)
                                    .ToList();

                    if(dbpagecontent != null)
                    {
                        context.PageContent.RemoveRange(dbpagecontent);
                    }

                    //ContentPermission
                    foreach (var pageContent in dbpagecontent)
                    {
                        var dbcontentPermission = context.ContentPermission
                                                .Where(p => p.PageContentId == pageContent.Id)
                                                .ToList();
                        if(dbcontentPermission != null)
                            context.ContentPermission.RemoveRange(dbcontentPermission);

                        //PageContentTranslation
                        var dbpagecontentTranslation = context.PageContentTranslation
                                                    .Where(p => p.PageContentId == pageContent.Id)
                                                    .ToList();
                        if (dbpagecontentTranslation != null)
                            context.PageContentTranslation.RemoveRange(dbpagecontentTranslation);      


                    }                    

                    //PageModule
                    var dbpageModule = context.PageModule
                                    .Where(p => p.PageId == id)
                                    .ToList();

                    if(dbpageModule != null)
                    {
                        context.PageModule.RemoveRange(dbpageModule);
                    }

                    //ModulePermission
                    foreach (var pageModule in dbpageModule)
                    {
                        var dbmodulePermission = context.ModulePermission
                                                .Where(p => p.PageModuleId == pageModule.Id)
                                                .ToList();
                        if (dbmodulePermission != null)
                            context.ModulePermission.RemoveRange(dbmodulePermission);
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting the page", ex);
            }
            return false;
        }

        private Entities.Page GetDeletedPage(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var page = context.Page
                        .Where(p => p.Id == id && p.IsDeleted).First();

                    return page;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while accessing deleted page", ex);
            }
            return null;

        }

        public bool DraftPage(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var permission = context.PagePermission
                        .Where(p => p.PageId == id && p.RoleId == Globals.AllUsersRoleId).FirstOrDefault();
                   
                    if (permission != null)
                        context.PagePermission.Remove(permission);

                    context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured while drafting the page", ex);
                return false;
            }            
        }

        public bool PublishPage(Guid id)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var permission = context.PagePermission
                     .Where(p => p.PageId == id && p.RoleId == Globals.AllUsersRoleId).FirstOrDefault();

                    if (permission == null)
                    {
                        Entities.PagePermission addpermission = new Entities.PagePermission();
                        addpermission.PageId = id;
                        addpermission.PermissionId = Globals.PageViewPermissionId;
                        addpermission.RoleId = Globals.AllUsersRoleId;                       
                        context.PagePermission.Add(addpermission);
                    }                        
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while publishing the page", ex);
                return false;
            }
            
        }
    }
}