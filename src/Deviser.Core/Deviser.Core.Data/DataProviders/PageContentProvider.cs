using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

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
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public PageContentProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration

        public PageContent Get(Guid pageContentId)
        {
            try
            {
                PageContent returnData = context.PageContent
                    .AsNoTracking()
                    .Include(pc=>pc.PageContentTranslation)
                   .Where(e => e.Id == pageContentId && e.IsDeleted == false)
                   .AsNoTracking()
                   .FirstOrDefault();
                return returnData;
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
                IEnumerable<PageContent> returnData = context.PageContent
                    .AsNoTracking()
                    .Where(e => e.ContainerId == containerId && e.IsDeleted == false)
                    .ToList();
                return new List<PageContent>(returnData);
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
                List<PageContent> returnData = context.PageContent
                    .Include(pc => pc.PageContentTranslation)
                    .Where(e => e.PageId == pageId)
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
                var returnData = context.PageContentTranslation
                    .Where(t => t.PageContentId == pageConentId && t.CultureCode == cultureCode)
                    .FirstOrDefault();
                return returnData;
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
                PageContentTranslation returnData = context.PageContentTranslation
                    .Where(t => t.Id== translationId)
                    .FirstOrDefault();
                return returnData;
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
                PageContent resultPageContent;
                //content.Id = Guid.NewGuid();
                content.CreatedDate = DateTime.Now;
                resultPageContent = context.PageContent.Add(content, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultPageContent;
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
                PageContentTranslation result;
                contentTranslation.CreatedDate = contentTranslation.LastModifiedDate = DateTime.Now;
                result = context.PageContentTranslation.Add(contentTranslation, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return result;
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
                PageContent resultPageContent;
                content.LastModifiedDate = DateTime.Now;
                resultPageContent = context.PageContent.Update(content, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultPageContent;
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
                foreach (var content in contents)
                {
                    content.LastModifiedDate = DateTime.Now;
                    if (context.PageContent.Any(pc => pc.Id == content.Id))
                    {
                        //content exist, therefore update the content 
                        context.PageContent.Update(content, GraphBehavior.SingleObject);
                    }
                    else
                    {
                        context.PageContent.Add(content, GraphBehavior.SingleObject);
                    }
                }
                context.SaveChanges();
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
                PageContentTranslation result;
                if (context.PageContentTranslation.Any(t => t.Id == contentTranslation.Id))
                {
                    contentTranslation.LastModifiedDate = DateTime.Now;
                    result = context.PageContentTranslation.Update(contentTranslation, GraphBehavior.SingleObject).Entity;
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

    }

}//End namespace
