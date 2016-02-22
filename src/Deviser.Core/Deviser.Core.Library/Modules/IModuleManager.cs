using Deviser.Core.Data.Entities;
using System.Collections.Generic;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageContent CreatePageContent(PageContent pageContent);
        PageModule CreatePageModule(PageModule pageModule);
        void UpdatePageModules(List<PageModule> pageModules);
    }
}