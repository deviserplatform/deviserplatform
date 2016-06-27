﻿using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Modules
{
    public class ModuleManager : IModuleManager
    {
        //Logger
        private readonly ILogger<ModuleManager> logger;

        IPageProvider pageProvider;
        IPageContentProvider pageContentProvider;
        ILifetimeScope container;

        public ModuleManager(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<ModuleManager>>();
            pageProvider = container.Resolve<IPageProvider>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
        }

        public List<PageModule> GetPageModuleByPage(int pageId)
        {
            try
            {
                var pageModules = pageProvider.GetPageModules(pageId);
                return pageModules;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting pageModules, pageId: ", pageId), ex);
            }
            
            return null;
        }

        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    PageModule result = pageProvider.GetPageModule(pageModule.Id);                    
                    if (result == null)
                        result = pageProvider.CreatePageModule(pageModule);
                    else 
                    {
                        result.IsDeleted = false;
                        result.ContainerId = pageModule.ContainerId;
                        result.SortOrder = pageModule.SortOrder;                        
                        result = pageProvider.UpdatePageModule(result);
                    }                        
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a pageModule, moduleId: ", pageModule.ModuleId), ex);
            }
            return null;
        }

        public void UpdatePageModules(List<PageModule> pageModules)
        {
            try
            {
                if (pageModules != null)
                {
                    pageProvider.UpdatePageModules(pageModules);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating pageModules"), ex);
            }
        }

        public PageContent CreatePageContent(PageContent pageContent)
        {
            try
            {
                PageContent result = pageContentProvider.Get(pageContent.Id);
                if (result == null)
                    result = pageContentProvider.Create(pageContent);
                else
                {                    
                    pageContent.IsDeleted = false;
                    result.ContainerId = pageContent.ContainerId;
                    result.SortOrder = pageContent.SortOrder;
                    result.LastModifiedDate = DateTime.Now;
                    result = pageContentProvider.Update(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page content"), ex);                
            }
            return null;
        }
    }
}
