using AutoMapper;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper.Execution;
using Deviser.Core.Data.Cache;

namespace Deviser.Core.Data.Repositories
{

    public interface IPageRepository //: IRepositoryBase
    {
        Page GetPageTree(bool isActiveOnly = false);

        Page GetPageTree(Guid pageId, bool isActiveOnly = false);
        //IList<Page> GetPages();
        IList<Page> GetPagesAndTranslations();
        IList<Page> GetDeletedPages();
        Page GetPage(Guid pageId);
        Page GetPageAndPagePermissions(Guid pageId);
        Page GetPageAndPageTranslations(Guid pageId);
        Page GetPageAndDependencies(Guid pageId, bool includeChild = true);
        IList<Page> GetPagesFlat(bool refreshCache = false);
        IList<PageType> GetPageTypes(bool refreshCache = false);
        Page CreatePage(Page dbPage);
        Page UpdatePageActiveAndLayout(Page page);
        Page UpdatePageAndPermissions(Page dbPage);
        Page UpdatePageTree(Page dbPage);
        Page RestorePage(Guid id);
        IList<PageTranslation> GetPageTranslations(string locale);
        PageTranslation GetPageTranslation(string url);
        IList<PageModule> GetPageModules(Guid pageId);
        IList<PageModule> GetDeletedPageModules();
        PageModule GetPageModule(Guid pageModuleId);
        //PageModule GetPageModuleByContainer(Guid containerId);
        PageModule CreatePageModule(PageModule dbPageModule);
        PageModule UpdatePageModule(PageModule dbPageModule);
        void UpdatePageModules(IList<PageModule> dbPageModules);
        void UpdateModulePermission(PageModule dbPageModule);
        IList<PagePermission> AddPagePermissions(IList<PagePermission> dbPagePermissions);
        IList<ModulePermission> AddModulePermissions(IList<ModulePermission> dbModulePermissions);
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
        private readonly IDeviserDataCache _deviserDataCache;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        private IList<string> _activeLanguages;
        //Constructor
        public PageRepository(DbContextOptions<DeviserDbContext> dbOptions,
            IDeviserDataCache deviserDataCache,
            ILogger<PageRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _deviserDataCache = deviserDataCache;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        //Custom Field Declaration
        public Page GetPageTree(bool isActiveOnly = false)
        {
            try
            {
                /*IEnumerable<> returnData = context.Pages.Include(x => x.ChildPages
                                                                    .Select(y => y.ChildPages.Select(c => c.ChildPages.Select(gc => gc.ChildPages.Select(ggc => ggc.ChildPages)))))
                                                                    .Where(.Where(e=>e.ParentId==null&&e.IsCurrentPage==false));*/
                /*IList<> returnData = new IList<>();
                returnData.Add(context.Pages.ToList().First());*/
                //var cacheName = nameof(GetPageTree);
                //var result = GetResultFromCache<Page>(cacheName);
                //if (result!=null)
                //{
                //    return result;
                //}

                //using var context = new DeviserDbContext(_dbOptions);
                var allPagesInFlat = GetPagesFlat()
                    .Where(p => isActiveOnly && p.IsActive || !isActiveOnly) //This Query cannot be translated to SQL by EF. Therefore, evaluating in client side.
                    .ToList();

                var rootOnly = allPagesInFlat.First(p => p.ParentId == null);

                GetPageTree(allPagesInFlat, rootOnly);
                var result = rootOnly;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting page tree", ex);
            }
            return null;
        }

        public Page GetPageTree(Guid pageId, bool isActiveOnly = false)
        {
            try
            {
                var allPagesInFlat = GetPagesFlat()
                    .Where(p => isActiveOnly && p.IsActive || !isActiveOnly) //This Query cannot be translated to SQL by EF. Therefore, evaluating in client side.
                    .ToList();

                var rootOnly = allPagesInFlat.First(p => p.Id == pageId);

                GetPageTree(allPagesInFlat, rootOnly);
                var result = rootOnly;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting page tree", ex);
            }
            return null;
        }

        private void GetPageTree(IList<Page> pagesInFlat, Page page)
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


        //public IList<Page> GetPages()
        //{
        //    try
        //    {
        //        //var cacheName = nameof(GetPages);
        //        //var result = GetResultFromCache<IList<Page>>(cacheName);
        //        //if (result != null)
        //        //{
        //        //    return result;
        //        //}

        //        using var context = new DeviserDbContext(_dbOptions);
        //        var dbResult = context.Page
        //            .Where(e => e.ParentId != null).AsNoTracking()
        //            //.Include("PageTranslations").Include("ChildPages").Include("PageModules").Include("PageModules.Module")
        //            .Include(p => p.PagePermissions)
        //            .Include(p => p.PageTranslation)
        //            .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
        //            .OrderBy(p => p.PageOrder)
        //            .AsNoTracking()
        //            .ToList();

        //        var result = _mapper.Map<IList<Page>>(dbResult);
        //        //AddResultToCache(cacheName, result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error occured while getting all pages", ex);
        //    }
        //    return null;
        //}

        /// <summary>
        /// Returns pages including AdminPage, AdminPage.Module, PageTranslation and PagePermissions
        /// </summary>
        /// <param name="refreshCache">Set "true" to refresh the cache</param>
        /// <returns></returns>
        public IList<Page> GetPagesFlat(bool refreshCache = false)
        {
            try
            {
                if (_deviserDataCache.ContainsKey(nameof(GetPagesFlat)) && !refreshCache)
                {
                    var cacheResult = _deviserDataCache.GetItem<IList<Entities.Page>>(nameof(GetPagesFlat));
                    return _mapper.Map<IList<Page>>(cacheResult);
                }

                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.Page
                    .AsNoTracking()
                    .Include(p => p.AdminPage).ThenInclude(ap => ap.Module)
                    .Include(p => p.PageTranslation)
                    .Include(p => p.PagePermissions)
                    .OrderBy(p => p.PageLevel).ThenBy(p => p.PageOrder)
                    .AsNoTracking()
                    .ToList();

                var result = _mapper.Map<IList<Page>>(dbResult);
                _deviserDataCache.AddOrUpdate(nameof(GetPagesFlat), dbResult);
                //AddResultToCache(cacheName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all pages", ex);
            }
            return null;

        }

        public IList<Page> GetPagesAndTranslations()
        {
            try
            {
                var result = GetPagesFlat()
                    .Where(e => e.ParentId != null)
                    .ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all pages", ex);
            }
            return null;

        }

        public IList<Page> GetDeletedPages()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Page
                    .Include(p => p.PageTranslation)
                    .Where(e => e.ParentId != null && !e.IsActive).AsNoTracking()
                    .ToList();

                return _mapper.Map<IList<Page>>(result);
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
                var result = GetPagesFlat()
                    .FirstOrDefault(e => e.Id == pageId);
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
                return GetPage(pageId);
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
                return GetPage(pageId);
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
                using var context = new DeviserDbContext(_dbOptions);
                Entities.Page dbResult;
                if (includeChild)
                {
                    dbResult = context.Page
                        .Where(e => e.Id == pageId).AsNoTracking()
                        .Include(p => p.PageType)
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
                        .AsNoTracking()
                        .FirstOrDefault();
                }
                else
                {
                    dbResult = context.Page.Where(e => e.Id == pageId).AsNoTracking().FirstOrDefault();
                }

                if (dbResult.PageModule != null)
                {
                    dbResult.PageModule = dbResult.PageModule.Where(pm => pm.IsActive).ToList();
                }

                if (dbResult.PageContent != null)
                {
                    dbResult.PageContent = dbResult.PageContent.Where(pc => pc.IsActive).ToList();
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

        public IList<PageType> GetPageTypes(bool refreshCache = false)
        {
            try
            {
                if (_deviserDataCache.ContainsKey(nameof(GetPageTypes)) && !refreshCache)
                {
                    var cacheResult = _deviserDataCache.GetItem<IList<Entities.PageType>>(nameof(GetPageTypes));
                    return _mapper.Map<IList<PageType>>(cacheResult);
                }


                using var context = new DeviserDbContext(_dbOptions);
                var dbResult = context.PageType
                    .AsNoTracking()
                    .ToList();

                var result = _mapper.Map<IList<PageType>>(dbResult);
                _deviserDataCache.AddOrUpdate(nameof(GetPageTypes), dbResult);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }

        public IList<PageTranslation> GetPageTranslations(string locale)
        {
            try
            {
                var pageTranslations = GetPageTranslation();
                var result = pageTranslations
                    .Where(e => e.Locale.ToLower() == locale.ToLower())
                    .OrderBy(p => p.PageId)
                    .ToList();
                return result;
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
                var pageTranslations = GetPageTranslation();

                var result = pageTranslations
                    .Where(e => string.Equals(e.URL, url, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(p => p.PageId)
                    .FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageTranslation", ex);
            }
            return null;
        }

        /// <summary>
        /// Creates new Method
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page CreatePage(Page page)
        {
            using var context = new DeviserDbContext(_dbOptions);
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var dbPage = _mapper.Map<Entities.Page>(page);
                dbPage.CreatedDate = DateTime.Now; dbPage.LastModifiedDate = DateTime.Now;
                var result = context.Page.Add(dbPage).Entity;
                context.SaveChanges();
                transaction.Commit();
                //Refresh cache
                GetPagesFlat(true);
                return _mapper.Map<Page>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling CreatePage", ex);
            }
            return null;
        }

        /// <summary>
        /// Updates following properties of the Page only:
        /// 1. Page.IsCurrentPage
        /// 2. Page.LayoutId
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePageActiveAndLayout(Page page)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPage = context.Page.First(p => p.Id == page.Id);
                dbPage.LayoutId = page.LayoutId;
                dbPage.IsActive = page.IsActive;
                var result = context.Page.Update(dbPage).Entity;
                context.SaveChanges();
                //Refresh cache
                GetPagesFlat(true);
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
            using var context = new DeviserDbContext(_dbOptions);
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var dbPage = _mapper.Map<Entities.Page>(page);

                var pagePermissions = dbPage.PagePermissions;
                //var pageTranslation = dbPage.PageTranslation;
                dbPage.PagePermissions = null;
                //dbPage.PageTranslation = null;

                //context.Page.Update(dbPage);

                if (pagePermissions != null & pagePermissions.Count > 0)
                {
                    //Filter deleted permissions in UI and delete all of them
                    var matchPagePermissions = context.PagePermission.Where(dbPermission => dbPermission.PageId == dbPage.Id)
                        //.AsNoTracking()
                        .ToList();

                    var toDelete = matchPagePermissions.Where(dbPermission =>
                        !pagePermissions.Any(pagePermission => pagePermission.PermissionId == dbPermission.PermissionId
                                                               && pagePermission.RoleId == dbPermission.RoleId
                                                               && pagePermission.RoleId == Globals.AdministratorRoleId)).ToList();

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


                UpdatePageAndPermissionsRecursive(dbPage, context);

                //context.PageTranslation.UpdateRange(page.PageTranslation);
                context.SaveChanges();
                transaction.Commit();

                //Refresh cache
                GetPagesFlat(true);
                var result = GetPageTree(page.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePage", ex);
            }
            return null;
        }

        /// <summary>
        /// Updates only PageLevel and PageOrder
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePageTree(Page page)
        {
            using var context = new DeviserDbContext(_dbOptions);
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var dbPage = _mapper.Map<Entities.Page>(page);
                dbPage.LastModifiedDate = DateTime.Now;
                UpdatePageTreeRecursive(context, dbPage);

                context.SaveChanges();
                transaction.Commit();

                //Refresh cache
                GetPagesFlat(true);
                return GetPageTree(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling UpdatePageTree", ex);
            }
            return null;
        }

        public IList<PageModule> GetPageModules(Guid pageId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageModule
                    .Where(e => e.PageId == pageId && e.IsActive)
                    .Include(e => e.Module)
                    .Include(e => e.ModuleAction).ThenInclude(mp => mp.ModuleActionProperties).ThenInclude(cp => cp.Property)
                    .Include(e => e.ModulePermissions)
                    .OrderBy(p => p.Id)
                    .ToList();

                return _mapper.Map<IList<PageModule>>(result);
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
                    .Where(e => e.Id == pageModuleId && e.IsActive)
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

        public IList<PageModule> GetDeletedPageModules()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageModule
                    .Include(P => P.Page).ThenInclude(P => P.PageTranslation)
                    .Where(e => !e.IsActive)
                    .OrderBy(p => p.Id)
                    .ToList();

                return _mapper.Map<IList<PageModule>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetPageModules", ex);
            }
            return null;
        }

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

        public void UpdatePageModules(IList<PageModule> pageModules)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageModules = _mapper.Map<IList<Entities.PageModule>>(pageModules);
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
        public IList<PagePermission> AddPagePermissions(IList<PagePermission> pagePermissions)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                if (pagePermissions != null && pagePermissions.Count > 0)
                {
                    var dbPagePermissions = _mapper.Map<IList<Entities.PagePermission>>(pagePermissions);
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
                    return _mapper.Map<IList<PagePermission>>(toAdd);
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
        public IList<ModulePermission> AddModulePermissions(IList<ModulePermission> pagePermissions)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                if (pagePermissions != null && pagePermissions.Count > 0)
                {
                    var dbModulePermissions = _mapper.Map<IList<Entities.ModulePermission>>(pagePermissions);
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
                    return _mapper.Map<IList<ModulePermission>>(toAdd);
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
                    dbPageModule.IsActive = true;
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
                var pageModule = context.PageModule.First(p => p.Id == id && !p.IsActive);

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
                    dbPage.IsActive = true;
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
                    .Where(p => p.PageId == id);

                context.PageTranslation.RemoveRange(dbPageTranslation);

                //PagePermission
                var dbPagePermission = context.PagePermission
                    .Where(p => p.PageId == id);

                context.PagePermission.RemoveRange(dbPagePermission);

                //PageContent
                var dbPageContent = context.PageContent
                    .Where(p => p.PageId == id);

                context.PageContent.RemoveRange(dbPageContent);

                //ContentPermission
                foreach (var pageContent in dbPageContent)
                {
                    var dbContentPermission = context.ContentPermission
                        .Where(p => p.PageContentId == pageContent.Id);
                    if (dbContentPermission != null)
                        context.ContentPermission.RemoveRange(dbContentPermission);

                    //PageContentTranslation
                    var dbPageContentTranslation = context.PageContentTranslation
                        .Where(p => p.PageContentId == pageContent.Id);

                    context.PageContentTranslation.RemoveRange(dbPageContentTranslation);


                }

                //PageModule
                var dbPageModule = context.PageModule
                    .Where(p => p.PageId == id);

                context.PageModule.RemoveRange(dbPageModule);

                //ModulePermission
                foreach (var pageModule in dbPageModule)
                {
                    var dbModulePermission = context.ModulePermission
                        .Where(p => p.PageModuleId == pageModule.Id)
                        .ToList();
                    context.ModulePermission.RemoveRange(dbModulePermission);
                }

                var adminPages = context.AdminPage.Where(ap => ap.PageId == id);
                context.RemoveRange(adminPages);

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting the page", ex);
            }
            return false;
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

        private static void UpdatePageTreeRecursive(DeviserDbContext context, Entities.Page page)
        {
            if (page == null || page.Id == Guid.Empty) return;

            var dbPage = context.Page
                .Include(p => p.PageTranslation)
                .First(p => p.Id == page.Id);
            dbPage.PageLevel = page.PageLevel;
            dbPage.PageOrder = page.PageOrder;
            dbPage.ParentId = page.ParentId;

            foreach (var pageTranslation in page.PageTranslation)
            {
                var dbPageTranslation = dbPage.PageTranslation.FirstOrDefault(pt =>
                    pt.PageId == pageTranslation.PageId && string.Equals(pt.Locale, pageTranslation.Locale,
                        StringComparison.InvariantCultureIgnoreCase));
                dbPageTranslation.URL = pageTranslation.URL;
            }

            context.SaveChanges();

            if (page.ChildPage == null || page.ChildPage.Count <= 0) return;

            foreach (var child in page.ChildPage.Select((value, index) => new { index, value }))
            {
                UpdatePageTreeRecursive(context, child.value);
            }
        }

        private Entities.Page GetDeletedPage(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var page = context.Page.First(p => p.Id == id && !p.IsActive);

                return page;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while accessing deleted page", ex);
            }
            return null;

        }

        private List<PageTranslation> GetPageTranslation()
        {
            var pageTranslations = new List<PageTranslation>();
            var pages = GetPagesAndTranslations().ToList();
            foreach (var page in pages)
            {
                pageTranslations.AddRange(page.PageTranslation);
            }

            return pageTranslations;
        }

        private void UpdatePageAndPermissionsRecursive(Entities.Page dbPage, DeviserDbContext context)
        {
            if (dbPage == null) return;

            //if (dbPage.AdminPage != null)
            //{
            //    dbPage.AdminPage.PageId = Guid.Empty;
            //}

            dbPage.LastModifiedDate = DateTime.Now;

            context.Page.Update(dbPage);
            //Update URL of child pages, if any
            if (dbPage.ChildPage.Count <= 0) return;

            foreach (var child in dbPage.ChildPage)
            {
                UpdatePageAndPermissionsRecursive(child, context);
            }
        }
    }
}