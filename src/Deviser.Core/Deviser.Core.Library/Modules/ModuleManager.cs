using Autofac;
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

        public PageModule CreatePageModule(PageModule pageModule)
        {
            try
            {
                if (pageModule != null)
                {
                    PageModule result = pageProvider.GetPageModuleByContainer(pageModule.ContainerId);                    
                    if (result == null)
                        result = pageProvider.CreatePageModule(pageModule);
                    else if(result.IsDeleted)
                    {
                        result.IsDeleted = false;
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
                    result = pageContentProvider.Update(pageContent);
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
