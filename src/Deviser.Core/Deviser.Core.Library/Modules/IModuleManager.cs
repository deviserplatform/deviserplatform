using Deviser.Core.Data.Entities;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageModule GetPageModule(Guid pageModuleId);
        List<PageModule> GetPageModuleByPage(Guid pageId);        
        PageModule CreateUpdatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);
        void UpdateModulePermission(PageModule pageModule);
        bool HasEditPermission(PageModule pageModule);
        bool HasViewPermission(PageModule pageModule);
    }
}