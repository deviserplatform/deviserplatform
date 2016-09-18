using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{

    public interface IPageContentProvider
    {
        PageContent Get(Guid pageContentId);
        //List<PageContent> GetByContainer(Guid containerId);
        List<PageContent> Get(Guid pageId, string cultureCode);
        PageContentTranslation GetTranslation(Guid pageConentId);
        PageContentTranslation GetTranslations(Guid pageConentId, string cultureCode);
        PageContent Create(PageContent content);
        PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation);
        PageContent Update(PageContent content);
        void AddOrUpdate(List<PageContent> contents);
        PageContentTranslation UpdateTranslation(PageContentTranslation contentTranslation);
        List<ContentPermission> AddContentPermissions(List<ContentPermission> contentPermissions);
        void UpdateContentPermission(PageContent pageContent);

    }

    public class PageContentProvider : DataProviderBase, IPageContentProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public PageContentProvider(ILifetimeScope container)
            : base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContent returnData = context.PageContent
                                .AsNoTracking()
                                .Include(pc => pc.PageContentTranslation)
                                .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentDataType)
                                .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties)
                                .Include(pc => pc.ContentPermissions)
                                .Where(e => e.Id == pageContentId && !e.IsDeleted)
                                .AsNoTracking()
                                .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    List<PageContent> returnData = context.PageContent
                               .Include(pc => pc.PageContentTranslation)
                               .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentDataType)
                               .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties).ThenInclude(ctp => ctp.Property)
                               .Include(pc => pc.ContentPermissions)
                               .Where(e => e.PageId == pageId && !e.IsDeleted)
                               .ToList();
                    foreach (var pageContent in returnData)
                    {
                        if (pageContent.PageContentTranslation != null)
                        {
                            pageContent.PageContentTranslation = pageContent.PageContentTranslation.Where(t => t.CultureCode == cultureCode).ToList();
                        }
                    }
                    return new List<PageContent>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PageContentTranslation
                                .Where(t => t.PageContentId == pageContentId && t.CultureCode == cultureCode && !t.IsDeleted)
                                .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        /// <summary>
        /// Get page content translations for given translationId
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public PageContentTranslation GetTranslation(Guid translationId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContentTranslation returnData = context.PageContentTranslation
                               .Where(t => t.Id == translationId && !t.IsDeleted)
                               .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting translation", ex);
            }
            return null;
        }

        /// <summary>
        /// Creates page content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public PageContent Create(PageContent content)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContent resultPageContent;
                    content.LastModifiedDate = content.CreatedDate = DateTime.Now;
                    resultPageContent = context.PageContent.Add(content).Entity;
                    context.SaveChanges();
                    return resultPageContent;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Create", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContentTranslation result;
                    contentTranslation.CreatedDate = contentTranslation.LastModifiedDate = DateTime.Now;
                    result = context.PageContentTranslation.Add(contentTranslation).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while Creating/Updating page content translation", ex);
            }
            return null;
        }

        /// <summary>
        /// Update page content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public PageContent Update(PageContent content)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContent resultPageContent;
                    content.LastModifiedDate = DateTime.Now;
                    resultPageContent = context.PageContent.Update(content).Entity;
                    context.SaveChanges();
                    return resultPageContent;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        /// <summary>
        /// It updates given list of page content if the content exisit in db, else it adds the content
        /// </summary>
        /// <param name="contents">
        /// List of page contents
        /// </param>
        public void AddOrUpdate(List<PageContent> contents)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    foreach (var content in contents)
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
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating contents", ex);
                throw ex;
            }
        }

        public PageContentTranslation UpdateTranslation(PageContentTranslation contentTranslation)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContentTranslation result;
                    if (context.PageContentTranslation.Any(t => t.Id == contentTranslation.Id))
                    {
                        contentTranslation.LastModifiedDate = DateTime.Now;
                        result = context.PageContentTranslation.Update(contentTranslation).Entity;
                        context.SaveChanges();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while Creating/Updating page content translation", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    if (contentPermissions != null && contentPermissions.Count > 0)
                    {
                        //Filter new permissions which are not in db and add all of them
                        var toAdd = contentPermissions.Where(contentPermission => !context.ContentPermission.Any(dbPermission =>
                        dbPermission.PermissionId == contentPermission.PermissionId &&
                        dbPermission.PageContentId == contentPermission.PageContentId &&
                        dbPermission.RoleId == contentPermission.RoleId)).ToList();
                        if (toAdd != null && toAdd.Count > 0)
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

        /// <summary>
        /// Update content permissions. It removes permissions which are not in parameters 
        /// and it adds permissions which are not in db
        /// </summary>
        /// <param name="contentPermissions"></param>
        public void UpdateContentPermission(PageContent pageContent)
        {
            if (pageContent.ContentPermissions!= null && pageContent.ContentPermissions.Count > 0)
            {
                //Assuming all permissions have same pageContentId
                var pageContentId = pageContent.Id;
                var contentPermissions = pageContent.ContentPermissions;
                try
                {
                    using (var context = new DeviserDBContext(dbOptions))
                    {

                        //Update InheritViewPermissions only
                        var dbPageContent = context.PageContent.First(pc => pc.Id == pageContentId);
                        dbPageContent.InheritViewPermissions = pageContent.InheritViewPermissions;
                        dbPageContent.InheritEditPermissions = pageContent.InheritEditPermissions;
                        
                        //Filter deleted permissions in UI and delete all of them
                        var toDelete = context.ContentPermission.Where(dbPermission => dbPermission.PageContentId == pageContentId &&
                        !contentPermissions.Any(contentPermission => contentPermission.PermissionId == dbPermission.PermissionId && contentPermission.RoleId == dbPermission.RoleId)).ToList();
                        if (toDelete != null && toDelete.Count > 0)
                            context.ContentPermission.RemoveRange(toDelete);

                        //Filter new permissions which are not in db and add all of them
                        var toAdd = contentPermissions.Where(contentPermission => !context.ContentPermission.Any(dbPermission =>
                        dbPermission.PermissionId == contentPermission.PermissionId &&
                        dbPermission.PageContentId == contentPermission.PageContentId &&
                        dbPermission.RoleId == contentPermission.RoleId)).ToList();
                        if (toAdd != null && toAdd.Count > 0)
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
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occured while updating module persmissions", ex);
                }
            }
        }

    }

}//End namespace
