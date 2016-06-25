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
        List<PageContent> GetByContainer(Guid containerId);
        List<PageContent> Get(int pageId, string cultureCode);
        PageContentTranslation GetTranslation(Guid pageConentId);
        PageContentTranslation GetTranslations(Guid pageConentId, string cultureCode);
        PageContent Create(PageContent content);
        PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation);
        PageContent Update(PageContent content);
        void Update(List<PageContent> contents);
        PageContentTranslation UpdateTranslation(PageContentTranslation contentTranslation);

    }

    public class PageContentProvider : DataProviderBase, IPageContentProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public PageContentProvider(ILifetimeScope container)
            :base(container)
        {            
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration

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
                                .Include(pc => pc.ContentType).ThenInclude(pc=>pc.ContentTypeProperties)
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
        public List<PageContent> GetByContainer(Guid containerId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<PageContent> returnData = context.PageContent
                               .AsNoTracking()
                               .Where(e => e.ContainerId == containerId && !e.IsDeleted)
                               .ToList();
                    return new List<PageContent>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetByContainer", ex);
            }
            return null;
        }
        public List<PageContent> Get(int pageId, string cultureCode)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    List<PageContent> returnData = context.PageContent
                               .Include(pc => pc.PageContentTranslation)
                               .Include(pc => pc.ContentType).ThenInclude(pc=> pc.ContentDataType)
                               .Include(pc => pc.ContentType).ThenInclude(pc => pc.ContentTypeProperties)
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
        public PageContentTranslation GetTranslations(Guid pageConentId, string cultureCode)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PageContentTranslation
                                .Where(t => t.PageContentId == pageConentId && t.CultureCode == cultureCode && !t.IsDeleted)
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
        public PageContent Create(PageContent content)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PageContent resultPageContent;
                    //content.Id = Guid.NewGuid();
                    content.CreatedDate = DateTime.Now;
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

        public void Update(List<PageContent> contents)
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

    }

}//End namespace
