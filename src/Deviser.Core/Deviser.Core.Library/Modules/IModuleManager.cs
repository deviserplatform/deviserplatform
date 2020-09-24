using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageModule GetPageModule(Guid pageModuleId);
        IList<PageModule> GetPageModuleByPage(Guid pageId);
        IList<PageModule> GetDeletedPageModules();
        PageModule CreateUpdatePageModule(PageModule pageModule);
        void UpdatePageModules(IList<PageModule> pageModules);
        PageModule UpdateModulePermission(PageModule pageModule);
        bool HasEditPermission(PageModule pageModule, bool isForCurrentRequest = false);
        bool HasViewPermission(PageModule pageModule, bool isForCurrentRequest = false);
    }
}