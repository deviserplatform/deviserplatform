using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{

    public interface IPageContentRepository
    {
        PageContent Get(Guid pageContentId);
        List<PageContent> Get(Guid pageId, string cultureCode);
        List<PageContent> Get();
        PageContent RestorePageContent(Guid id);
        PageContentTranslation GetTranslation(Guid pageContentId);
        PageContentTranslation GetTranslations(Guid pageContentId, string cultureCode);
        PageContent Create(PageContent dbPageContent);
        PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation);
        PageContent Update(PageContent content);
        void AddOrUpdate(List<PageContent> dbPageContents);
        PageContentTranslation UpdateTranslation(PageContentTranslation dbPageContentTranslation);
        List<ContentPermission> AddContentPermissions(List<ContentPermission> dbContentPermissions);
        void UpdateContentPermission(PageContent dbPageContentSrc);
        bool DeletePageContent(Guid id);

    }

    public class PageContentRepository : IPageContentRepository
    {
        //Logger
        private readonly ILogger<PageContentRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public PageContentRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<PageContentRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        /// <summary>
        /// Get page content by pageContentId
        /// </summary>
        /// <param name="pageContentId"></param>
        /// <returns></returns>
        public PageContent Get(Guid pageContentId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageContent
                    .AsNoTracking()
                    .Include(pc => pc.PageContentTranslation)
                    .Include(pc => pc.ContentType)
                    .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties).ThenInclude(ctp => ctp.Property)
                    .Include(pc => pc.ContentPermissions)
                    .Where(e => e.Id == pageContentId && !e.IsDeleted)
                    .AsNoTracking()
                    .FirstOrDefault();
                return _mapper.Map<PageContent>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        ///// <summary>
        ///// Get page contents by container
        ///// </summary>
        ///// <param name="containerId"></param>
        ///// <returns></returns>
        //public List<PageContent> GetByContainer(Guid containerId)
        //{
        //    try
        //    {
        //        using (var context = new DeviserDBContext(dbOptions))
        //        {
        //            IEnumerable<PageContent> returnData = context.PageContent
        //                       .AsNoTracking()
        //                       .Where(e => e.ContainerId == containerId && !e.IsDeleted)
        //                       .ToList();
        //            return new List<PageContent>(returnData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("Error occured while getting GetByContainer", ex);
        //    }
        //    return null;
        //}

        /// <summary>
        /// Get all page contents for given pageId and cultureCode.
        /// Including PageContentTranslation, ContentType, ContentDataType, ContentTypeProperties, Property 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public List<PageContent> Get(Guid pageId, string cultureCode)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageContent
                    .Include(pc => pc.PageContentTranslation)
                    .Include(pc => pc.ContentType)
                    .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties).ThenInclude(ctp => ctp.Property)
                    .Include(pc => pc.ContentPermissions)
                    .Where(e => e.PageId == pageId && !e.IsDeleted)
                    .ToList();

                foreach (var pageContent in result.Where(pageContent => pageContent.PageContentTranslation != null))
                {
                    pageContent.PageContentTranslation = pageContent.PageContentTranslation.Where(t => t.CultureCode == cultureCode).ToList();
                }

                return _mapper.Map<List<PageContent>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public List<PageContent> Get()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageContent
                    .Include(p => p.Page).ThenInclude(p => p.PageTranslation)
                    .Include(pc => pc.ContentPermissions)
                    .Where(p => p.IsDeleted)
                    .OrderBy(p => p.Id)
                    .ToList();

                return _mapper.Map<List<PageContent>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting deleted page contents", ex);
            }
            return null;
        }

        /// <summary>
        /// Get page content translation for given pageContentId and cultureCode
        /// </summary>
        /// <param name="pageContentId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public PageContentTranslation GetTranslations(Guid pageContentId, string cultureCode)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageContentTranslation
                    .FirstOrDefault(t => t.PageContentId == pageContentId && t.CultureCode == cultureCode && !t.IsDeleted);
                return _mapper.Map<PageContentTranslation>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        /// <summary>
        /// Get page content translations for given translationId
        /// </summary>
        /// <param name="pageContentId"></param>
        /// <returns></returns>
        public PageContentTranslation GetTranslation(Guid pageContentId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.PageContentTranslation
                    .FirstOrDefault(t => t.Id == pageContentId && !t.IsDeleted);

                return _mapper.Map<PageContentTranslation>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting translation", ex);
            }
            return null;
        }

        /// <summary>
        /// Creates page content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        public PageContent Create(PageContent pageContent)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContent = _mapper.Map<Entities.PageContent>(pageContent);
                dbPageContent.LastModifiedDate = dbPageContent.CreatedDate = DateTime.Now;
                var result = context.PageContent.Add(dbPageContent).Entity;
                context.SaveChanges();
                return _mapper.Map<PageContent>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }

        /// <summary>
        /// Create page translation
        /// </summary>
        /// <param name="contentTranslation"></param>
        /// <returns></returns>
        public PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContentTranslation = _mapper.Map<Entities.PageContentTranslation>(contentTranslation);
                dbPageContentTranslation.CreatedDate = dbPageContentTranslation.LastModifiedDate = DateTime.Now;
                var result = context.PageContentTranslation.Add(dbPageContentTranslation).Entity;
                context.SaveChanges();
                return _mapper.Map<PageContentTranslation>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while Creating/Updating page content translation", ex);
            }
            return null;
        }

        /// <summary>
        /// Update page content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        public PageContent Update(PageContent pageContent)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContent = _mapper.Map<Entities.PageContent>(pageContent);
                dbPageContent.LastModifiedDate = DateTime.Now;
                dbPageContent.ContentType.ContentTypeProperties = null;
                var result = context.PageContent.Update(dbPageContent).Entity;
                context.SaveChanges();
                return _mapper.Map<PageContent>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        /// <summary>
        /// It updates given list of page content if the content exisit in db, else it adds the content
        /// </summary>
        /// <param name="pageContents">
        /// List of page contents
        /// </param>
        public void AddOrUpdate(List<PageContent> pageContents)
        {
            try
            {
                var dbPageContents = _mapper.Map<List<Entities.PageContent>>(pageContents);
                using var context = new DeviserDbContext(_dbOptions);
                foreach (var content in dbPageContents)
                {
                    content.LastModifiedDate = DateTime.Now;
                    if (context.PageContent.Any(pc => pc.Id == content.Id))
                    {
                        //content exist, therefore update the content 
                        context.PageContent.Update(content);
                    }
                    else
                    {
                        context.PageContent.Add(content);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating contents", ex);
                throw;
            }
        }

        public PageContentTranslation UpdateTranslation(PageContentTranslation pageContentTranslation)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContentTranslation = _mapper.Map<Entities.PageContentTranslation>(pageContentTranslation);
                if (context.PageContentTranslation.Any(t => t.Id == dbPageContentTranslation.Id))
                {
                    dbPageContentTranslation.LastModifiedDate = DateTime.Now;
                    var result = context.PageContentTranslation.Update(dbPageContentTranslation).Entity;
                    context.SaveChanges();
                    return _mapper.Map<PageContentTranslation>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while Creating/Updating page content translation", ex);
            }
            return null;
        }

        /// <summary>
        /// Add content permissions
        /// </summary>
        /// <param name="contentPermissions"></param>
        /// <returns></returns>
        public List<ContentPermission> AddContentPermissions(List<ContentPermission> contentPermissions)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbContentPermissions = _mapper.Map<List<Entities.ContentPermission>>(contentPermissions);
                if (dbContentPermissions != null && dbContentPermissions.Count > 0)
                {
                    //Filter new permissions which are not in db and add all of them
                    var toAdd = dbContentPermissions.Where(contentPermission => !context.ContentPermission.Any(dbPermission =>
                        dbPermission.PermissionId == contentPermission.PermissionId &&
                        dbPermission.PageContentId == contentPermission.PageContentId &&
                        dbPermission.RoleId == contentPermission.RoleId)).ToList();
                    if (toAdd.Count > 0)
                    {
                        foreach (var permission in toAdd)
                        {
                            //permission.Page = null;
                            if (permission.Id == Guid.Empty)
                                permission.Id = Guid.NewGuid();
                            context.ContentPermission.Add(permission);
                        }
                    }

                    context.SaveChanges();
                    return _mapper.Map<List<ContentPermission>>(contentPermissions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while addming module persmissions", ex);
            }
            return null;
        }

        /// <summary>
        /// Update content permissions. It removes permissions which are not in parameters 
        /// and it adds permissions which are not in db
        /// </summary>
        /// <param name="pageContent"></param>
        public void UpdateContentPermission(PageContent pageContent)
        {
            if (pageContent.ContentPermissions == null || pageContent.ContentPermissions.Count <= 0) return;

            var dbPageContentSrc = _mapper.Map<Entities.PageContent>(pageContent);
            //Assuming all permissions have same pageContentId
            var pageContentId = dbPageContentSrc.Id;
            var contentPermissions = dbPageContentSrc.ContentPermissions;
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                //Update InheritViewPermissions only
                var dbPageContent = context.PageContent.First(pc => pc.Id == pageContentId);
                dbPageContent.InheritViewPermissions = dbPageContentSrc.InheritViewPermissions;
                dbPageContent.InheritEditPermissions = dbPageContentSrc.InheritEditPermissions;

                //Filter deleted permissions in UI and delete all of them
                var toDelete = context.ContentPermission.Where(dbPermission => dbPermission.PageContentId == pageContentId &&
                                                                               !contentPermissions.Any(contentPermission => contentPermission.PermissionId == dbPermission.PermissionId && contentPermission.RoleId == dbPermission.RoleId)).ToList();
                if (toDelete.Count > 0)
                    context.ContentPermission.RemoveRange(toDelete);

                //Filter new permissions which are not in db and add all of them
                var toAdd = contentPermissions.Where(contentPermission => !context.ContentPermission.Any(dbPermission =>
                    dbPermission.PermissionId == contentPermission.PermissionId &&
                    dbPermission.PageContentId == contentPermission.PageContentId &&
                    dbPermission.RoleId == contentPermission.RoleId)).ToList();
                if (toAdd.Count > 0)
                {
                    foreach (var permission in toAdd)
                    {
                        //permission.Page = null;
                        if (permission.Id == Guid.Empty)
                            permission.Id = Guid.NewGuid();
                        context.ContentPermission.Add(permission);
                    }
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating module persmissions", ex);
                throw;
            }
        }

        public PageContent RestorePageContent(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContent = GetDeletedPageContent(id);

                if (dbPageContent != null)
                {
                    dbPageContent.IsDeleted = false;
                    var result = context.PageContent.Update(dbPageContent).Entity;
                    context.SaveChanges();
                    return _mapper.Map<PageContent>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while restoring page content", ex);
            }
            return null;
        }

        public bool DeletePageContent(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbPageContent = GetDeletedPageContent(id);

                if (dbPageContent != null)
                {
                    context.PageContent.Remove(dbPageContent);

                    //ContentPermission
                    var contentPermission = context.ContentPermission
                        .Where(p => p.PageContentId == id)
                        .ToList();
                    context.ContentPermission.RemoveRange(contentPermission);

                    //PageContentTranslation
                    var contentTranslation = context.PageContentTranslation
                        .Where(p => p.PageContentId == dbPageContent.Id)
                        .ToList();
                    context.PageContentTranslation.RemoveRange(contentTranslation);

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting page content", ex);
            }
            return false;
        }

        private Entities.PageContent GetDeletedPageContent(Guid id)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var pageContent = context.PageContent.First(p => p.Id == id && p.IsDeleted);

                return pageContent;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while accessing page content", ex);
            }
            return null;
        }

    }

}//End namespace
