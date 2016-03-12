using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;


namespace Deviser.Core.Data.DataProviders
{

    public interface IPageProvider
    {
        Page GetPageTree();
        List<Page> GetPages();
        Page GetPage(int pageId);
        Page CreatePage(Page page);
        Page UpdatePage(Page page);
        Page UpdatePageTree(Page page);
        List<PageTranslation> GetPageTranslations(string locale);
        List<PageModule> GetPageModules(int pageId);
        PageModule GetPageModule(Guid pageModuleId);
        PageModule GetPageModuleByContainer(Guid containerId);
        PageModule CreatePageModule(PageModule pageModule);
        PageModule UpdatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);

    }

    public class PageProvider : DataProviderBase, IPageProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public PageProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
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

                return context.Page
                    .Include(p => p.PageTranslation) //("PageTranslations")  
                    .ToList().First();
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageTree", ex);
            }
            return null;
        }
        public List<Page> GetPages()
        {
            try
            {
                IEnumerable<Page> returnData = context.Page
                    .Where(e => e.ParentId == null)
                    //.Include("PageTranslations").Include("ChildPages").Include("PageModules").Include("PageModules.Module")
                    .Include(p => p.PageTranslation).Include(p => p.ChildPage)
                    .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                    .OrderBy(p => p.Id)
                    .ToList();

                return new List<Page>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPages", ex);
            }
            return null;
        }

        public Page GetPage(int pageId)
        {
            try
            {
                Page returnData = context.Page
                   .Where(e => e.Id == pageId)
                   //.Include("PageTranslations").Include("Layout").Include("PageContents").Include("PageModules")
                   .Include(p => p.PageTranslation)
                   .Include(p => p.Layout)
                   .Include(p => p.PageContent).ThenInclude(pc=>pc.PageContentTranslation)
                   .Include(p => p.PageModule).ThenInclude(pm => pm.Module)
                   .OrderBy(p => p.Id)
                   .FirstOrDefault();

                return returnData;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPage", ex);
            }
            return null;
        }
        public Page CreatePage(Page page)
        {
            try
            {
                Page resultPage;
                page.CreatedDate = DateTime.Now; page.LastModifiedDate = DateTime.Now;
                resultPage = context.Page.Add(page, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultPage;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreatePage", ex);
            }
            return null;
        }
        public Page UpdatePage(Page page)
        {
            try
            {
                Page resultPage;
                page.LastModifiedDate = DateTime.Now;
                //resultPage = context.UpdateGraph<Page>(page, map => map.OwnedCollection(p => p.PageTranslations));
                resultPage = context.Page.Update(page, GraphBehavior.IncludeDependents).Entity;
                context.SaveChanges();
                return resultPage;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePage", ex);
            }
            return null;
        }
        public Page UpdatePageTree(Page page)
        {
            try
            {
                Page resultPage = null;
                page.LastModifiedDate = DateTime.Now;
                UpdatePageTreeTree(context, page);
                context.SaveChanges();
                resultPage = context.Page.ToList().First();
                return resultPage;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageTree", ex);
            }
            return null;
        }

        private void UpdatePageTreeTree(DeviserDBContext context, Page page)
        {
            if (page != null && page.ChildPage != null)
            {
                //context.UpdateGraph(page, map => map.AssociatedCollection(p => p.ChildPages)
                //                                  .OwnedCollection(p => p.PageTranslations));

                context.Page.Update(page, GraphBehavior.IncludeDependents);
                context.SaveChanges();

                if (page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                        if (child != null && child.ChildPage != null)
                        {
                            if (child.ChildPage.Count > 0)
                            {
                                //foreach (var grandChild in child.ChildPages)
                                UpdatePageTreeTree(context, child);
                            }
                            else if (child.Id > 0)
                            {
                                //context.UpdateGraph(child, map => map.AssociatedCollection(p => p.ChildPages)
                                //                  .OwnedCollection(p => p.PageTranslations));
                                context.Page.Update(page, GraphBehavior.IncludeDependents);
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
                IEnumerable<PageTranslation> returnData = context.PageTranslation
                    .Where(e => e.Locale.ToLower() == locale.ToLower())
                    .OrderBy(p => p.PageId)
                    .ToList();
                return new List<PageTranslation>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageTranslations", ex);
            }
            return null;
        }
        public List<PageModule> GetPageModules(int pageId)
        {
            try
            {
                IEnumerable<PageModule> returnData = context.PageModule
                    .Where(e => e.PageId == pageId)
                    .OrderBy(p => p.Id)
                    .ToList();

                return new List<PageModule>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetPageModules", ex);
            }
            return null;
        }

        public PageModule GetPageModule(Guid pageModuleId)
        {
            try
            {
                PageModule returnData = context.PageModule
                   .Where(e => e.Id == pageModuleId)
                   .OrderBy(p => p.Id)
                   .FirstOrDefault();

                return returnData;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }

        public PageModule GetPageModuleByContainer(Guid containerId)
        {
            try
            {
                PageModule returnData = context.PageModule
                   .Where(e => e.ContainerId == containerId)
                   .OrderBy(p => p.Id)
                   .FirstOrDefault();

                return returnData;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling GetPageModule", ex);
            }
            return null;
        }
        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                PageModule resultPageModule;
                resultPageModule = context.PageModule.Add(pageModule, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultPageModule;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling CreatePageModule", ex);
            }
            return null;
        }
        public PageModule UpdatePageModule(PageModule pageModule)
        {
            try
            {
                PageModule resultPageModule;
                resultPageModule = context.PageModule.Attach(pageModule, GraphBehavior.SingleObject).Entity;
                context.Entry(pageModule).State = EntityState.Modified;
                context.SaveChanges();
                return resultPageModule;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageModule", ex);
            }
            return null;
        }

        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                foreach(var module in pageModules)
                {
                    if(context.PageModule.Any(pm=>pm.Id == module.Id))
                    {
                        //page module exist, therefore update it
                        context.PageModule.Update(module, GraphBehavior.SingleObject);
                    }
                    else
                    {
                        context.PageModule.Add(module);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling UpdatePageModules", ex);
            }
        }        
    }
}