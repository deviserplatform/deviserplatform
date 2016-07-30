using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageModule GetPageModule(Guid pageModuleId);
        List<PageModule> GetPageModuleByPage(Guid pageId);        
        PageContent CreatePageContent(PageContent pageContent);
        PageModule CreateUpdatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);
    }
}