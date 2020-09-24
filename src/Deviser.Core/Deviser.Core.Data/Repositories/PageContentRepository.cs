using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Deviser.Core.Data.Repositories
{
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
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageContent
                .AsNoTracking()
                .Include(pc => pc.PageContentTranslation)
                .Include(pc => pc.ContentType).ThenInclude(ct => ct.ContentTypeFields).ThenInclude(ctf => ctf.ContentFieldType)
                .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties).ThenInclude(ctp => ctp.Property)
                .Include(pc => pc.ContentPermissions)
                .Where(e => e.Id == pageContentId && e.IsActive)
                .AsNoTracking()
                .FirstOrDefault();
            return _mapper.Map<PageContent>(result);
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
        //                       .Where(e => e.ContainerId == containerId && e.IsActive)
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
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageContent
                .Include(pc => pc.PageContentTranslation)
                .Include(pc => pc.ContentType).ThenInclude(ct => ct.ContentTypeFields).ThenInclude(ctf => ctf.ContentFieldType)
                .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties).ThenInclude(ctp => ctp.Property)
                .Include(pc => pc.ContentPermissions)
                .Where(e => e.PageId == pageId && e.IsActive)
                .ToList();

            foreach (var pageContent in result.Where(pageContent => pageContent.PageContentTranslation != null))
            {
                pageContent.PageContentTranslation = pageContent.PageContentTranslation.Where(t => t.CultureCode == cultureCode).ToList();
            }

            return _mapper.Map<List<PageContent>>(result);
        }

        public List<PageContent> GetDeletedPageContents()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageContent
                .Include(p => p.Page).ThenInclude(p => p.PageTranslation)
                .Include(pc => pc.ContentPermissions)
                .Where(p => !p.IsActive)
                .OrderBy(p => p.Id)
                .ToList();

            return _mapper.Map<List<PageContent>>(result);
        }

        /// <summary>
        /// Get page content translation for given pageContentId and cultureCode
        /// </summary>
        /// <param name="pageContentId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public PageContentTranslation GetTranslations(Guid pageContentId, string cultureCode)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageContentTranslation
                .FirstOrDefault(t => t.PageContentId == pageContentId && t.CultureCode == cultureCode && !t.IsActive);
            return _mapper.Map<PageContentTranslation>(result);
        }

        /// <summary>
        /// Get page content translations for given translationId
        /// </summary>
        /// <param name="pageContentId"></param>
        /// <returns></returns>
        public PageContentTranslation GetTranslation(Guid pageContentId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.PageContentTranslation
                .FirstOrDefault(t => t.Id == pageContentId && t.IsActive);

            return _mapper.Map<PageContentTranslation>(result);
        }

        /// <summary>
        /// Creates page content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        public PageContent Create(PageContent pageContent)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContent = _mapper.Map<Entities.PageContent>(pageContent);
            dbPageContent.LastModifiedDate = dbPageContent.CreatedDate = DateTime.Now;
            var result = context.PageContent.Add(dbPageContent).Entity;
            context.SaveChanges();
            return _mapper.Map<PageContent>(result);
        }

        /// <summary>
        /// Create page translation
        /// </summary>
        /// <param name="contentTranslation"></param>
        /// <returns></returns>
        public PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContentTranslation = _mapper.Map<Entities.PageContentTranslation>(contentTranslation);
            dbPageContentTranslation.CreatedDate = dbPageContentTranslation.LastModifiedDate = DateTime.Now;
            var result = context.PageContentTranslation.Add(dbPageContentTranslation).Entity;
            context.SaveChanges();
            return _mapper.Map<PageContentTranslation>(result);
        }

        /// <summary>
        /// Update page content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        public PageContent Update(PageContent pageContent)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContent = _mapper.Map<Entities.PageContent>(pageContent);
            dbPageContent.LastModifiedDate = DateTime.Now;
            dbPageContent.ContentType.ContentTypeProperties = null;
            var result = context.PageContent.Update(dbPageContent).Entity;
            context.SaveChanges();
            return _mapper.Map<PageContent>(result);
        }

        /// <summary>
        /// It updates given list of page content if the content exisit in db, else it adds the content
        /// </summary>
        /// <param name="pageContents">
        /// List of page contents
        /// </param>
        public void AddOrUpdate(List<PageContent> pageContents)
        {
            var dbPageContents = _mapper.Map<List<Entities.PageContent>>(pageContents);
            using var context = new DeviserDbContext(_dbOptions);
            var pageContentIds = context.PageContent.AsNoTracking().Select(pc => pc.Id).ToHashSet();
            foreach (var content in dbPageContents)
            {
                content.LastModifiedDate = DateTime.Now;
                //if (context.PageContent.AsNoTracking().Any(pc => pc.Id == content.Id))
                if (pageContentIds.Contains(content.Id))
                {
                    //content exist, therefore update the content 
                    var pageContent = context.PageContent.First(pc => pc.Id == content.Id);
                    pageContent.ContainerId = content.ContainerId;
                    pageContent.SortOrder = content.SortOrder;
                }
                else
                {
                    context.PageContent.Add(new Entities.PageContent()
                    {
                        Id = content.Id,
                        PageId = content.PageId,
                        ContainerId = content.ContainerId,
                        ContentTypeId = content.ContentTypeId,
                        SortOrder = content.SortOrder,
                        IsActive = true,
                        InheritEditPermissions = true,
                        InheritViewPermissions = true
                    });

                    var adminPermissions = new List<Entities.ContentPermission>()
                        {
                            new Entities.ContentPermission()
                            {
                                PageContentId = content.Id,
                                RoleId = Globals.AdministratorRoleId,
                                PermissionId = Globals.ContentViewPermissionId,
                            },
                            new Entities.ContentPermission()
                            {
                                PageContentId = content.Id,
                                RoleId = Globals.AdministratorRoleId,
                                PermissionId = Globals.ContentEditPermissionId,
                            }
                        };

                    AddContentPermissions(context, adminPermissions);
                }
            }
            context.SaveChanges();

        }

        public PageContentTranslation UpdateTranslation(PageContentTranslation pageContentTranslation)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContentTranslation = _mapper.Map<Entities.PageContentTranslation>(pageContentTranslation);
            if (!context.PageContentTranslation.Any(t => t.Id == dbPageContentTranslation.Id)) throw new InvalidOperationException($"Parameter {nameof(pageContentTranslation)} is not found");

            dbPageContentTranslation.LastModifiedDate = DateTime.Now;
            var result = context.PageContentTranslation.Update(dbPageContentTranslation).Entity;
            context.SaveChanges();
            return _mapper.Map<PageContentTranslation>(result);

        }

        /// <summary>
        /// Add content permissions
        /// </summary>
        /// <param name="contentPermissions"></param>
        /// <returns></returns>
        public List<ContentPermission> AddContentPermissions(List<ContentPermission> contentPermissions)
        {
            if (contentPermissions == null || contentPermissions.Count <= 0) throw new InvalidOperationException($"Invalid parameter {nameof(contentPermissions)}");

            using var context = new DeviserDbContext(_dbOptions);
            var dbContentPermissions = _mapper.Map<List<Entities.ContentPermission>>(contentPermissions);
            //Filter new permissions which are not in db and add all of them
            AddContentPermissions(context, dbContentPermissions);
            context.SaveChanges();
            return _mapper.Map<List<ContentPermission>>(contentPermissions);
        }

        private static void AddContentPermissions(DeviserDbContext context, List<Entities.ContentPermission> dbContentPermissions)
        {
            var toAdd = dbContentPermissions.Where(contentPermission => !context.ContentPermission.Any(dbPermission =>
                dbPermission.PermissionId == contentPermission.PermissionId &&
                dbPermission.PageContentId == contentPermission.PageContentId &&
                dbPermission.RoleId == contentPermission.RoleId)).ToList();
            if (toAdd.Count <= 0) return;

            foreach (var permission in toAdd)
            {
                //permission.Page = null;
                if (permission.Id == Guid.Empty)
                    permission.Id = Guid.NewGuid();
                context.ContentPermission.Add(permission);
            }
        }

        /// <summary>
        /// Update content permissions. It removes permissions which are not in parameters 
        /// and it adds permissions which are not in db
        /// </summary>
        /// <param name="pageContent"></param>
        public PageContent UpdateContentPermission(PageContent pageContent)
        {
            if (pageContent.ContentPermissions == null || pageContent.ContentPermissions.Count <= 0)
                throw new InvalidOperationException("PageContent ContentPermissions cannot be null or empty");

            var dbPageContentSrc = _mapper.Map<Entities.PageContent>(pageContent);
            //Assuming all permissions have same pageContentId
            var pageContentId = dbPageContentSrc.Id;
            var contentPermissions = dbPageContentSrc.ContentPermissions;

            using var context = new DeviserDbContext(_dbOptions);
            //Update InheritViewPermissions only
            var dbPageContent = context.PageContent.First(pc => pc.Id == pageContentId);
            dbPageContent.InheritViewPermissions = dbPageContentSrc.InheritViewPermissions;
            dbPageContent.InheritEditPermissions = dbPageContentSrc.InheritEditPermissions;

            //Filter deleted permissions in UI and delete all of them
            var toDelete = context.ContentPermission
                               .Where(dbPermission => dbPermission.PageContentId == pageContentId)
                               .ToList()
                               .Where(dbPermission => !contentPermissions.Any(contentPermission => contentPermission.PermissionId == dbPermission.PermissionId && contentPermission.RoleId == dbPermission.RoleId))
                               .ToList();

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
            return Get(pageContent.Id);
        }

        public PageContent RestorePageContent(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContent = GetDeletedPageContent(id);
            if (dbPageContent == null) throw new InvalidOperationException($"Page content is not found {id}");

            dbPageContent.IsActive = true;
            var result = context.PageContent.Update(dbPageContent).Entity;
            context.SaveChanges();
            return _mapper.Map<PageContent>(result);
        }

        public bool DeletePageContent(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbPageContent = GetDeletedPageContent(id);

            if (dbPageContent == null) throw new InvalidOperationException($"Page content is not found {id}");

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

        private Entities.PageContent GetDeletedPageContent(Guid id)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var pageContent = context.PageContent.FirstOrDefault(p => p.Id == id && !p.IsActive);
            return pageContent;
        }

    }

}//End namespace
