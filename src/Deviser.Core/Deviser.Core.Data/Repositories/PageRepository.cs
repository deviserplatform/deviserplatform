using AutoMapper;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{

    public interface IPageRepository //: IRepositoryBase
    {
        Page GetPageTree();
        List<Page> GetPages();
        List<Page> GetDeletedPages();
        Page GetPage(Guid pageId);
        Page GetPageAndPagePermissions(Guid pageId);
        Page GetPageAndPageTranslations(Guid pageId);
        Page GetPageAndDependencies(Guid pageId, bool includeChild = true);
        Page CreatePage(Page dbPage);
        Page UpdatePage(Page page);
        Page UpdatePageAndPermissions(Page dbPage);
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

    public class PageRepository : IPageRepository
    {
        //Logger
        private readonly ILogger<PageRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public PageRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<PageRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
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
                //var cacheName = nameof(GetPageTree);
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result!=null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                var allPagesInFlat = context.Page
                    .Include(p => p.AdminPage).ThenInclude(ap => ap.Module)
                    .Include(p => p.PageTranslation)
                    .Include(p => p.PagePermissions)
                    .AsNoTracking().ToList();

                var rootOnly = allPagesInFlat.First(p => p.ParentId == null);

                GetPageTree(allPagesInFlat, rootOnly);
                var result = _mapper.Map<Page>(rootOnly);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting page tree", ex);
            }
            return null;
        }

        private void GetPageTree(List<Entities.Page> pagesInFlat, Entities.Page page)
        {
            //Page resultPage = null;

            page.ChildPage = pagesInFlat
                .Where(p => p.ParentId == page.Id)
                .OrderBy(p => p.PageOrder)
                .ToList();

            if (page.ChildPage != null)
            {
                foreach (var child in page.ChildPage)
                {
                    GetPageTree(pagesInFlat, child);
                }
            }
            //return resultPage;
        }

        public List<Page> GetPages()
        {
            try
            {
                //var cacheName = nameof(GetPages);
                //var result = GetResultFromCache<List<Page>>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.Page
                    .Where(e => e.ParentId != null).AsNoTracking()
                    //.Include("PageTranslations").Include("ChildPages").Include("PageModules").Include("PageModules.Module")
                    .Include(p => p.PagePermissions)
                    .Include(p => p.PageTranslation)
                    .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                    .OrderBy(p => p.PageOrder)
                    .ToList();

                var result = _mapper.Map<List<Page>>(dbResult);
                //AddResultToCache(cacheName, result);
                return result;
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Page
                    .Include(p => p.PageTranslation)
                    .Where(e => e.ParentId != null && e.IsDeleted).AsNoTracking()
                    .ToList();

                return _mapper.Map<List<Page>>(result);
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
                //var cacheName = $"{nameof(GetPage)}_{pageId}";
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.Page
                    .Where(e => e.Id == pageId).AsNoTracking()
                    .FirstOrDefault();

                var result = _mapper.Map<Page>(dbResult);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }

        public Page GetPageAndPagePermissions(Guid pageId)
        {
            try
            {
                //var cacheName = $"{nameof(GetPageAndPagePermissions)}_{pageId}";
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.Page
                    .Where(e => e.Id == pageId).AsNoTracking()
                    .Include(p => p.PagePermissions)
                    .FirstOrDefault();

                var result = _mapper.Map<Page>(dbResult);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPageAndPagePermissions", ex);
            }
            return null;
        }

        public Page GetPageAndPageTranslations(Guid pageId)
        {
            try
            {
                //var cacheName = $"{nameof(GetPageAndPageTranslations)}_{pageId}";
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.Page
                    .Where(e => e.Id == pageId).AsNoTracking()
                    .Include(p => p.PageTranslation)
                    .FirstOrDefault();

                var result = _mapper.Map<Page>(dbResult);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPageAndPageTranslations", ex);
            }
            return null;
        }

        public Page GetPageAndDependencies(Guid pageId, bool includeChild = true)
        {
            try
            {
                //var cacheName = $"{nameof(GetPageAndDependencies)}_{pageId}";
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using var context = new DeviserDbContext(_dbOptions);
                Entities.Page dbResult;
                if (includeChild)
                {
                    dbResult = context.Page
                        .Where(e => e.Id == pageId).AsNoTracking()
                        .Include(p => p.AdminPage).ThenInclude(ap => ap.Module)
                        .Include(p => p.PageTranslation)
                        .Include(p => p.PagePermissions)
                        .Include(p => p.Layout)
                        .Include(p => p.PageContent).ThenInclude(pc => pc.PageContentTranslation)
                        .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType)
                        .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct => ct.ContentTypeProperties).ThenInclude(ctp => ctp.Property).ThenInclude(p => p.OptionList)
                        .Include(p => p.PageContent).ThenInclude(pc => pc.ContentPermissions)
                        .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                        .Include(p => p.PageModule).ThenInclude(pm => pm.ModulePermissions)
                        .OrderBy(p => p.Id)
                        .FirstOrDefault();
                }
                else
                {
                    dbResult = context.Page.Where(e => e.Id == pageId).AsNoTracking().FirstOrDefault();
                }



                if (dbResult.PageModule != null)
                {
                    dbResult.PageModule = dbResult.PageModule.Where(pm => !pm.IsDeleted).ToList();
                }

                if (dbResult.PageContent != null)
                {
                    dbResult.PageContent = dbResult.PageContent.Where(pc => !pc.IsDeleted).ToList();
                }

                var result = _mapper.Map<Page>(dbResult);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPageAndDependencies", ex);
            }
            return null;
        }

        public Page CreatePage(Page page)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = _mapper.Map<Entities.Page>(page);
                dbPage.CreatedDate = DateTime.Now; dbPage.LastModifiedDate = DateTime.Now;
                var result = context.Page.Add(dbPage).Entity;
                context.SaveChanges();
                return _mapper.Map<Page>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling CreatePage", ex);
            }
            return null;
        }

        /// <summary>
        /// Updates Page only
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePage(Page page)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = _mapper.Map<Entities.Page>(page);
                dbPage.LastModifiedDate = DateTime.Now;
                dbPage.PagePermissions = null;
                dbPage.PageTranslation = null;

                var result = context.Page.Update(dbPage).Entity;
                context.SaveChanges();
                return _mapper.Map<Page>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePage", ex);
            }
            return null;
        }

        /// <summary>
        /// Updates page and add/remove permissions
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePageAndPermissions(Page page)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = _mapper.Map<Entities.Page>(page);

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
                    var matchPagePermissions = context.PagePermission.Where(dbPermission => dbPermission.PageId == dbPage.Id)
                        .ToList();

                    var toDelete = matchPagePermissions.Where(dbPermission =>
                        !pagePermissions.Any(pagePermission => pagePermission.PermissionId == dbPermission.PermissionId && pagePermission.RoleId == dbPermission.RoleId)).ToList();

                    if (toDelete.Count > 0)
                        context.PagePermission.RemoveRange(toDelete);

                    //Filter new permissions which are not in db and add all of them
                    var toAdd = pagePermissions.Where(pagePermission => !context.PagePermission.Any(dbPermission =>
                        dbPermission.PermissionId == pagePermission.PermissionId &&
                        dbPermission.PageId == pagePermission.PageId &&
                        dbPermission.RoleId == pagePermission.RoleId)).ToList();
                    if (toAdd.Count > 0)
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
                return _mapper.Map<Page>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = _mapper.Map<Entities.Page>(page);
                dbPage.LastModifiedDate = DateTime.Now;
                UpdatePageTreeTree(context, dbPage);
                context.SaveChanges();
                var result = context.Page.ToList().First();
                return _mapper.Map<Page>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePageTree", ex);
            }
            return null;
        }

        private static void UpdatePageTreeTree(DeviserDbContext context, Entities.Page page)
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageTranslation
                    .Where(e => e.Locale.ToLower() == locale.ToLower())
                    .OrderBy(p => p.PageId)
                    .ToList();
                return _mapper.Map<List<PageTranslation>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageTranslation
                    .Where(e => string.Equals(e.URL, url, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(p => p.PageId)
                    .FirstOrDefault();
                return _mapper.Map<PageTranslation>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageModule
                    .Where(e => e.PageId == pageId && !e.IsDeleted)
                    .Include(e => e.Module)
                    .Include(e => e.ModuleAction).ThenInclude(mp => mp.ModuleActionProperties).ThenInclude(cp => cp.Property)
                    .Include(e => e.ModulePermissions)
                    .OrderBy(p => p.Id)
                    .ToList();

                return _mapper.Map<List<PageModule>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageModule
                    .Include(pm => pm.ModulePermissions)
                    .Include(pm => pm.Module)
                    .Include(e => e.ModuleAction).ThenInclude(ma => ma.ModuleActionProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                    .Where(e => e.Id == pageModuleId && !e.IsDeleted)
                    .OrderBy(p => p.Id)
                    .FirstOrDefault();

                return _mapper.Map<PageModule>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }

        public List<PageModule> GetDeletedPageModules()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageModule
                    .Include(P => P.Page).ThenInclude(P => P.PageTranslation)
                    .Where(e => e.IsDeleted)
                    .OrderBy(p => p.Id)
                    .ToList();

                return _mapper.Map<List<PageModule>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
                var result = context.PageModule.Add(dbPageModule).Entity;
                context.SaveChanges();
                return _mapper.Map<PageModule>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
                var result = context.PageModule.Update(dbPageModule).Entity;
                context.SaveChanges();
                return _mapper.Map<PageModule>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModules = _mapper.Map<List<Entities.PageModule>>(pageModules);
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
                using var context = new DeviserDbContext(_dbOptions);
                if (pagePermissions != null && pagePermissions.Count > 0)
                {
                    var dbPagePermissions = _mapper.Map<List<Entities.PagePermission>>(pagePermissions);
                    //Filter new permissions which are not in db and add all of them
                    var toAdd = dbPagePermissions.Where(pagePermission => !context.PagePermission.Any(dbPermission =>
                        dbPermission.PermissionId == pagePermission.PermissionId &&
                        dbPermission.PageId == pagePermission.PageId &&
                        dbPermission.RoleId == pagePermission.RoleId)).ToList();
                    if (toAdd.Count > 0)
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
                    return _mapper.Map<List<PagePermission>>(toAdd);
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
                using var context = new DeviserDbContext(_dbOptions);
                if (pagePermissions != null && pagePermissions.Count > 0)
                {
                    var dbModulePermissions = _mapper.Map<List<Entities.ModulePermission>>(pagePermissions);
                    //Filter new permissions which are not in db and add all of them
                    var toAdd = dbModulePermissions.Where(modulePermission => !context.ModulePermission.Any(dbPermission =>
                        dbPermission.PermissionId == modulePermission.PermissionId &&
                        dbPermission.PageModuleId == modulePermission.PageModuleId &&
                        dbPermission.RoleId == modulePermission.RoleId)).ToList();
                    if (toAdd.Count > 0)
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
                    return _mapper.Map<List<ModulePermission>>(toAdd);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while adding module permissions", ex);
            }
            return null;
        }

        public void UpdateModulePermission(PageModule pageModule)
        {
            if (pageModule.ModulePermissions != null && pageModule.ModulePermissions.Count > 0)
            {
                var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
                //Assuming all permissions have same pageModuleId
                var pageModuleId = dbPageModule.Id;
                var modulePermissions = dbPageModule.ModulePermissions;

                try
                {
                    using var context = new DeviserDbContext(_dbOptions);
                    //Update InheritViewPermissions only
                    var dbPageContent = context.PageModule.First(pc => pc.Id == pageModuleId);
                    dbPageContent.InheritViewPermissions = dbPageModule.InheritViewPermissions;
                    dbPageContent.InheritEditPermissions = dbPageModule.InheritEditPermissions;

                    //Filter deleted permissions in UI and delete all of them
                    var toDelete = context.ModulePermission.Where(dbPermission => dbPermission.PageModuleId == pageModuleId &&
                                                                                  !modulePermissions.Any(modulePermission => modulePermission.PermissionId == dbPermission.PermissionId && modulePermission.RoleId == dbPermission.RoleId)).ToList();
                    if (toDelete.Count > 0)
                        context.ModulePermission.RemoveRange(toDelete);

                    //Filter new permissions which are not in db and add all of them
                    var toAdd = modulePermissions.Where(modulePermission => !context.ModulePermission.Any(dbPermission =>
                        dbPermission.PermissionId == modulePermission.PermissionId &&
                        dbPermission.PageModuleId == modulePermission.PageModuleId &&
                        dbPermission.RoleId == modulePermission.RoleId)).ToList();
                    if (toAdd.Count > 0)
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
                catch (Exception ex)
                {
                    _logger.LogError("Error occured while updating module permissions", ex);
                    throw;
                }
            }
        }

        public PageModule RestorePageModule(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModule = GetDeletedPageModule(id);

                if (dbPageModule != null)
                {
                    dbPageModule.IsDeleted = false;
                    var result = context.PageModule.Update(dbPageModule).Entity;
                    context.SaveChanges();
                    return _mapper.Map<PageModule>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModule = GetDeletedPageModule(id);

                if (dbPageModule != null)
                {
                    context.PageModule.Remove(dbPageModule);
                    var pageModulePermissions = context.ModulePermission
                        .Where(p => p.PageModuleId == id)
                        .ToList();
                    context.ModulePermission.RemoveRange(pageModulePermissions);
                    context.SaveChanges();
                    return true;
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
                using var context = new DeviserDbContext(_dbOptions);
                var pageModule = context.PageModule.First(p => p.Id == id && p.IsDeleted);

                return pageModule;
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = GetDeletedPage(id);

                if (dbPage != null)
                {
                    dbPage.IsDeleted = false;
                    var result = context.Update(dbPage).Entity;
                    context.SaveChanges();
                    return _mapper.Map<Page>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                //Page
                var dbPage = GetDeletedPage(id);
                if (dbPage != null)
                {
                    context.Page.Remove(dbPage);
                }

                //PageTranslation
                var dbPageTranslation = context.PageTranslation
                    .Where(p => p.PageId == id)
                    .ToList();

                context.PageTranslation.RemoveRange(dbPageTranslation);

                //PagePermission
                var dbPagePermission = context.PagePermission
                    .Where(p => p.PageId == id)
                    .ToList();

                context.PagePermission.RemoveRange(dbPagePermission);

                //PageContent
                var dbPageContent = context.PageContent
                    .Where(p => p.PageId == id)
                    .ToList();

                context.PageContent.RemoveRange(dbPageContent);

                //ContentPermission
                foreach (var pageContent in dbPageContent)
                {
                    var dbContentPermission = context.ContentPermission
                        .Where(p => p.PageContentId == pageContent.Id)
                        .ToList();
                    if (dbContentPermission != null)
                        context.ContentPermission.RemoveRange(dbContentPermission);

                    //PageContentTranslation
                    var dbPageContentTranslation = context.PageContentTranslation
                        .Where(p => p.PageContentId == pageContent.Id)
                        .ToList();
                    context.PageContentTranslation.RemoveRange(dbPageContentTranslation);


                }

                //PageModule
                var dbPageModule = context.PageModule
                    .Where(p => p.PageId == id)
                    .ToList();

                context.PageModule.RemoveRange(dbPageModule);

                //ModulePermission
                foreach (var pageModule in dbPageModule)
                {
                    var dbModulePermission = context.ModulePermission
                        .Where(p => p.PageModuleId == pageModule.Id)
                        .ToList();
                    context.ModulePermission.RemoveRange(dbModulePermission);
                }

                context.SaveChanges();
                return true;
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
                using var context = new DeviserDbContext(_dbOptions);
                var page = context.Page.First(p => p.Id == id && p.IsDeleted);

                return page;
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
                using var context = new DeviserDbContext(_dbOptions);
                var permission = context.PagePermission.FirstOrDefault(p => p.PageId == id && p.RoleId == Globals.AllUsersRoleId);

                if (permission != null)
                    context.PagePermission.Remove(permission);

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while drafting the page", ex);
                return false;
            }
        }

        public bool PublishPage(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var permission = context.PagePermission.FirstOrDefault(p => p.PageId == id && p.RoleId == Globals.AllUsersRoleId);

                if (permission == null)
                {
                    var addPermission = new Entities.PagePermission
                    {
                        PageId = id,
                        PermissionId = Globals.PageViewPermissionId,
                        RoleId = Globals.AllUsersRoleId
                    };
                    context.PagePermission.Add(addPermission);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while publishing the page", ex);
                return false;
            }

        }
    }
}