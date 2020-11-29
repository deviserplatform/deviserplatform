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
using AutoMapper.Internal;
using Deviser.Core.Data.Cache;

namespace Deviser.Core.Data.Repositories
{
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
            var allPagesInFlat = GetPagesFlat()
                .Where(p => isActiveOnly && p.IsActive || !isActiveOnly) //This Query cannot be translated to SQL by EF. Therefore, evaluating in client side.
                .ToList();

            var rootOnly = allPagesInFlat.First(p => p.ParentId == null);

            GetPageTree(allPagesInFlat, rootOnly);
            var result = rootOnly;
            return result;
        }

        public Page GetPageTree(Guid pageId, bool isActiveOnly = false)
        {
            var allPagesInFlat = GetPagesFlat()
                .Where(p => isActiveOnly && p.IsActive || !isActiveOnly) //This Query cannot be translated to SQL by EF. Therefore, evaluating in client side.
                .ToList();

            var rootOnly = allPagesInFlat.First(p => p.Id == pageId);

            GetPageTree(allPagesInFlat, rootOnly);
            var result = rootOnly;
            return result;
        }

        private void GetPageTree(IList<Page> pagesInFlat, Page page)
        {
            page.ChildPage = pagesInFlat
                .Where(p => p.ParentId == page.Id)
                .OrderBy(p => p.PageOrder)
                .ToList();

            if (page.ChildPage == null) return;

            foreach (var child in page.ChildPage)
            {
                GetPageTree(pagesInFlat, child);
            }
        }

        /// <summary>
        /// Returns pages including AdminPage, AdminPage.Module, PageTranslation and PagePermissions
        /// </summary>
        /// <param name="refreshCache">Set "true" to refresh the cache</param>
        /// <returns></returns>
        public IList<Page> GetPagesFlat(bool refreshCache = false)
        {
            if (_deviserDataCache.ContainsKey(nameof(GetPagesFlat)) && !refreshCache)
            {
                var cacheResult = _deviserDataCache.GetItem<IList<Entities.Page>>(nameof(GetPagesFlat));
                return _mapper.Map<IList<Page>>(cacheResult);
            }

            using var context = new DeviserDbContext(_dbOptions);
            var query = context.Page
                .AsNoTracking()
                .Include(p => p.AdminPage).ThenInclude(ap => ap.Module)
                .Include(p => p.PageTranslation)
                .Include(p => p.PagePermissions)
                .OrderBy(p => p.PageLevel).ThenBy(p => p.PageOrder);


            var dbResult = query.ToList();
            var result = _mapper.Map<IList<Page>>(dbResult);
            _deviserDataCache.AddOrUpdate(nameof(GetPagesFlat), dbResult);
            //AddResultToCache(cacheName, result);
            return result;

        }

        public IList<Page> GetPagesAndTranslations()
        {
            var result = GetPagesFlat()
                .Where(e => e.ParentId != null)
                .ToList();
            return result;
        }

        public IList<Page> GetDeletedPages()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Page
                .Include(p => p.PageTranslation)
                .Where(e => e.ParentId != null && !e.IsActive).AsNoTracking()
                .ToList();

            return _mapper.Map<IList<Page>>(result);
        }

        public Page GetPage(Guid pageId)
        {
            var result = GetPagesFlat()
                .FirstOrDefault(e => e.Id == pageId);
            return result;
        }

        public Page GetPageAndPagePermissions(Guid pageId)
        {
            return GetPage(pageId);
        }

        public Page GetPageAndPageTranslations(Guid pageId)
        {
            return GetPage(pageId);
        }

        public Page GetPageAndDependencies(Guid pageId, bool includeChild = true)
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
                    .Include(p => p.PageContent).ThenInclude(pc => pc.ContentType).ThenInclude(ct => ct.ContentTypeFields)
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

        public IList<PageType> GetPageTypes(bool refreshCache = false)
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

        public IList<PageTranslation> GetPageTranslations(string locale)
        {
            var pageTranslations = GetPageTranslation();
            var result = pageTranslations
                .Where(e => e.Locale.ToLower() == locale.ToLower())
                .OrderBy(p => p.PageId)
                .ToList();
            return result;
        }

        public PageTranslation GetPageTranslation(string url)
        {
            var pageTranslations = GetPageTranslation();

            var result = pageTranslations
                .Where(e => string.Equals(e.URL, url, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(p => p.PageId)
                .FirstOrDefault();
            return result;
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
            var dbPage = _mapper.Map<Entities.Page>(page);
            dbPage.IsActive = true;
            dbPage.CreatedDate = DateTime.Now; dbPage.LastModifiedDate = DateTime.Now;
            var result = context.Page.Add(dbPage).Entity;
            context.SaveChanges();
            transaction.Commit();
            //Refresh cache
            GetPagesFlat(true);
            return _mapper.Map<Page>(result);
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

        /// <summary>
        /// Updates page and add/remove permissions
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page UpdatePageAndPermissions(Page page)
        {
            using var context = new DeviserDbContext(_dbOptions);
            using var transaction = context.Database.BeginTransaction();
            var pageEntity = _mapper.Map<Entities.Page>(page);

            var pagePermissions = pageEntity.PagePermissions;
            //var pageTranslation = pageEntity.PageTranslation;
            pageEntity.PagePermissions = null;
            //pageEntity.PageTranslation = null;

            //context.Page.Update(pageEntity);

            if (pagePermissions != null & pagePermissions.Count > 0)
            {
                //Filter deleted permissions in UI and delete all of them
                var matchPagePermissions = context.PagePermission.Where(dbPermission => dbPermission.PageId == pageEntity.Id)
                    .AsNoTracking()
                    .ToList();

                var toDelete = matchPagePermissions.Where(dbPermission =>
                    dbPermission.RoleId != Globals.AdministratorRoleId && //Skip Administrator role from delete process
                    !pagePermissions.Any(pagePermission => pagePermission.PermissionId == dbPermission.PermissionId
                                                           && pagePermission.RoleId == dbPermission.RoleId)).ToList();

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

            var sourcePageTranslations = new Dictionary<Guid, Dictionary<string, Entities.PageTranslation>>();
            GetFlatPagesFromRoot(pageEntity, sourcePageTranslations);

            //Update selected page
            var dbPage = context.Page
                .FirstOrDefault(p => p.Id == page.Id);
            _mapper.Map(pageEntity, dbPage);

            var pageIds = sourcePageTranslations.Keys.ToHashSet();

            var dbPageTranslations = context.PageTranslation
                .Where(pt => pageIds.Contains(pt.PageId))
                .ToList()
                .GroupBy(pt => pt.PageId)
                .ToDictionary(g => g.Key, g => g.ToDictionary(g => g.Locale, g => g));

            //Update PageTranslation and children page translations
            foreach (var sourcePageTranslation in sourcePageTranslations)
            {
                var destPageTranslations = dbPageTranslations[sourcePageTranslation.Key];

                foreach (var pageTranslationKvp in sourcePageTranslation.Value)
                {
                    if (destPageTranslations.ContainsKey(pageTranslationKvp.Key))
                    {
                        var dbPageTranslation = destPageTranslations[pageTranslationKvp.Key];
                        _mapper.Map(pageTranslationKvp.Value, dbPageTranslation);
                    }
                    else
                    {
                        context.PageTranslation.Add(pageTranslationKvp.Value);
                    }
                }
            }

            if (pageEntity.AdminPage != null)
            {
                var dbAdminPage = context.AdminPage.FirstOrDefault(p => p.PageId == pageEntity.Id);
                if (dbAdminPage == null)
                {
                    pageEntity.AdminPage.PageId = pageEntity.Id;
                    context.AdminPage.Add(pageEntity.AdminPage);
                }
                else
                {
                    dbAdminPage.ModuleId = pageEntity.AdminPage.ModuleId;
                    dbAdminPage.ModelName = pageEntity.AdminPage.ModelName;
                }
            }

            ////UpdatePageAndTranslationRecursive(pageEntity, context);

            ////context.PageTranslation.UpdateRange(pageEntity.PageTranslation);
            context.SaveChanges();
            transaction.Commit();

            //Refresh cache
            GetPagesFlat(true);
            var result = GetPageTree(page.Id);
            return result;
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
            var dbPage = _mapper.Map<Entities.Page>(page);
            dbPage.LastModifiedDate = DateTime.Now;
            UpdatePageTreeRecursive(context, dbPage);

            context.SaveChanges();
            transaction.Commit();

            //Refresh cache
            GetPagesFlat(true);
            return GetPageTree(true);
        }

        public IList<PageModule> GetPageModules(Guid pageId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageModule
                .Where(e => e.PageId == pageId && e.IsActive)
                .Include(e => e.Module)
                .Include(e => e.ModuleView).ThenInclude(mp => mp.ModuleViewProperties).ThenInclude(cp => cp.Property)
                .Include(e => e.ModulePermissions)
                .OrderBy(p => p.Id)
                .ToList();

            return _mapper.Map<IList<PageModule>>(result);
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageModule
                .Include(pm => pm.ModulePermissions)
                .Include(pm => pm.Module)
                .Include(e => e.ModuleView).ThenInclude(ma => ma.ModuleViewProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                .Where(e => e.Id == pageModuleId && e.IsActive)
                .OrderBy(p => p.Id)
                .FirstOrDefault();

            return _mapper.Map<PageModule>(result);
        }

        public IList<PageModule> GetDeletedPageModules()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageModule
                .Include(P => P.Page).ThenInclude(P => P.PageTranslation)
                .Where(e => !e.IsActive)
                .OrderBy(p => p.Id)
                .ToList();

            return _mapper.Map<IList<PageModule>>(result);
        }

        public PageModule CreatePageModule(PageModule pageModule)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
            var result = context.PageModule.Add(dbPageModule).Entity;
            context.SaveChanges();
            return _mapper.Map<PageModule>(result);
        }

        public PageModule UpdatePageModule(PageModule pageModule)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
            var result = context.PageModule.Update(dbPageModule).Entity;
            context.SaveChanges();
            return _mapper.Map<PageModule>(result);
        }

        public void AddOrUpdatePageModules(IList<PageModule> pageModules)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageModules = _mapper.Map<IList<Entities.PageModule>>(pageModules);
            var pageModuleIds = context.PageModule.AsNoTracking().Select(pc => pc.Id).ToHashSet();
            foreach (var pageModule in dbPageModules)
            {
                //if (context.PageModule.Any(pm => pm.Id == pageModule.Id))
                if (pageModuleIds.Contains(pageModule.Id))
                {
                    var dbPageModule = context.PageModule.First(pc => pc.Id == pageModule.Id);
                    dbPageModule.ContainerId = pageModule.ContainerId;
                    dbPageModule.SortOrder = pageModule.SortOrder;
                }
                else
                {
                    context.PageModule.Add(new Entities.PageModule()
                    {
                        Id = pageModule.Id,
                        PageId = pageModule.PageId,
                        ContainerId = pageModule.ContainerId,
                        ModuleId = pageModule.ModuleId,
                        ModuleViewId = pageModule.ModuleViewId,
                        SortOrder = pageModule.SortOrder,
                        IsActive = true,
                        InheritEditPermissions = true,
                        InheritViewPermissions = true
                    });

                    var adminPermissions = new List<Entities.ModulePermission>()
                        {
                            new Entities.ModulePermission()
                            {
                                PageModuleId = pageModule.Id,
                                RoleId = Globals.AdministratorRoleId,
                                PermissionId = Globals.ModuleViewPermissionId,
                            },
                            new Entities.ModulePermission()
                            {
                                PageModuleId  = pageModule.Id,
                                RoleId = Globals.AdministratorRoleId,
                                PermissionId = Globals.ModuleEditPermissionId,
                            }
                        };
                    context.ModulePermission.AddRange(adminPermissions);
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Add permissions only if its not exisit in db
        /// </summary>
        /// <param name="pagePermissions"></param>
        /// <returns></returns>
        public IList<PagePermission> AddPagePermissions(IList<PagePermission> pagePermissions)
        {
            using var context = new DeviserDbContext(_dbOptions);
            if (pagePermissions == null || pagePermissions.Count <= 0) throw new InvalidOperationException($"Invalid parameter, Parameter {nameof(pagePermissions)} cannot be empty");

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

        /// <summary>
        /// Add permissions only if its not exist in db
        /// </summary>
        /// <param name="pagePermissions"></param>
        /// <returns></returns>
        public IList<ModulePermission> AddModulePermissions(IList<ModulePermission> pagePermissions)
        {
            using var context = new DeviserDbContext(_dbOptions);
            if (pagePermissions == null || pagePermissions.Count <= 0) throw new InvalidOperationException($"Invalid parameter, Parameter {nameof(pagePermissions)} cannot be empty");
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

        public PageModule UpdateModulePermission(PageModule pageModule)
        {
            if (pageModule.ModulePermissions == null || pageModule.ModulePermissions.Count <= 0)
                throw new InvalidOperationException("PageModule ModulePermissions cannot be null or empty");

            var dbPageModule = _mapper.Map<Entities.PageModule>(pageModule);
            //Assuming all permissions have same pageModuleId
            var pageModuleId = dbPageModule.Id;
            var modulePermissions = dbPageModule.ModulePermissions;

            using var context = new DeviserDbContext(_dbOptions);
            //Update InheritViewPermissions only
            var dbPageContent = context.PageModule.First(pc => pc.Id == pageModuleId);
            dbPageContent.InheritViewPermissions = dbPageModule.InheritViewPermissions;
            dbPageContent.InheritEditPermissions = dbPageModule.InheritEditPermissions;

            //Filter deleted permissions in UI and delete all of them
            var toDelete = context.ModulePermission
                .Where(dbPermission => dbPermission.PageModuleId == pageModuleId)
                .ToList()
                .Where(dbPermission => !modulePermissions.Any(modulePermission => modulePermission.PermissionId == dbPermission.PermissionId && modulePermission.RoleId == dbPermission.RoleId))
                .ToList();

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
            return GetPageModule(pageModule.Id);
        }

        public PageModule RestorePageModule(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageModule = GetDeletedPageModule(id);
            if (dbPageModule == null) throw new InvalidOperationException($"Page Module cannot be found {id}");

            dbPageModule.IsActive = true;
            var result = context.PageModule.Update(dbPageModule).Entity;
            context.SaveChanges();
            return _mapper.Map<PageModule>(result);
        }

        public bool DeletePageModule(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageModule = GetDeletedPageModule(id);

            if (dbPageModule == null) throw new InvalidOperationException($"Page Module cannot be found {id}");
            context.PageModule.Remove(dbPageModule);
            var pageModulePermissions = context.ModulePermission
                .Where(p => p.PageModuleId == id)
                .ToList();
            context.ModulePermission.RemoveRange(pageModulePermissions);
            context.SaveChanges();
            return true;
        }

        private Entities.PageModule GetDeletedPageModule(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var pageModule = context.PageModule.First(p => p.Id == id && !p.IsActive);

            return pageModule;
        }

        public Page RestorePage(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPage = GetDeletedPage(id);
            if (dbPage == null) throw new InvalidOperationException($"Page cannot be not found {id}");

            dbPage.IsActive = true;
            var result = context.Update(dbPage).Entity;
            context.SaveChanges();
            return _mapper.Map<Page>(result);
        }

        public bool DeletePage(Guid id)
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

        public bool DraftPage(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var permission = context.PagePermission.FirstOrDefault(p => p.PageId == id && p.RoleId == Globals.AllUsersRoleId);

            if (permission != null)
                context.PagePermission.Remove(permission);

            context.SaveChanges();
            return true;
        }

        public bool PublishPage(Guid id)
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
            using var context = new DeviserDbContext(_dbOptions);
            var page = context.Page.First(p => p.Id == id && !p.IsActive);
            return page;
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

        private void GetFlatPagesFromRoot(Entities.Page pageEntity, Dictionary<Guid, Dictionary<string, Entities.PageTranslation>> flatPages)
        {
            if (pageEntity == null) return;

            var pageTranslations = pageEntity.PageTranslation.ToDictionary(pt => pt.Locale, pt => pt);
            flatPages.Add(pageEntity.Id, pageTranslations);

            //Update URL of child pages, if any
            if (pageEntity.ChildPage.Count <= 0) return;

            foreach (var child in pageEntity.ChildPage)
            {
                GetFlatPagesFromRoot(child, flatPages);
            }
        }

        private void UpdatePageAndTranslationRecursive(Entities.Page pageEntity, DeviserDbContext context)
        {
            if (pageEntity == null) return;
            pageEntity.LastModifiedDate = DateTime.Now;

            var dbPage = context.Page.Where(p => p.Id == pageEntity.Id).FirstOrDefault();

            _mapper.Map(pageEntity, dbPage);

            //context.Page.Update(pageEntity);
            //context.PageTranslation.UpdateRange(pageEntity.PageTranslation);

            foreach (var translationEntity in pageEntity.PageTranslation)
            {
                translationEntity.PageId = pageEntity.Id;

                var dbPageTranslation = context.PageTranslation.FirstOrDefault(pt =>
                    pt.PageId == pageEntity.Id && pt.Locale == translationEntity.Locale);

                if (dbPageTranslation != null)
                {
                    _mapper.Map(translationEntity, dbPageTranslation);
                }
                else
                {
                    context.PageTranslation.Add(translationEntity);
                }
            }

            //Update URL of child pages, if any
            if (pageEntity.ChildPage.Count <= 0) return;

            foreach (var child in pageEntity.ChildPage)
            {
                UpdatePageAndTranslationRecursive(child, context);
            }
        }
    }
}