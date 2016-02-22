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
        PageContent Create(PageContent content);
        PageContent Update(PageContent content);
        void Update(List<PageContent> contents);

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
                IEnumerable<PageContent> returnData = context.PageContent
                    .Where(e => e.PageId == pageId && e.CultureCode == cultureCode)
                    .ToList();
                return new List<PageContent>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
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
                contents.ForEach(c => c.LastModifiedDate = DateTime.Now);
                context.PageContent.UpdateRange(contents, GraphBehavior.SingleObject);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating contents", ex);
            }
        }

    }

}//End namespace
