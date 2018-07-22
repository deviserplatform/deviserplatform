using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageModule GetPageModule(Guid pageModuleId);
        List<PageModule> GetPageModuleByPage(Guid pageId);
        List<PageModule> GetDeletedPageModules();
        PageModule CreateUpdatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);
        void UpdateModulePermission(PageModule pageModule);
        bool HasEditPermission(PageModule pageModule, bool isForCurrentRequest = false);
        bool HasViewPermission(PageModule pageModule, bool isForCurrentRequest = false);
    }
}