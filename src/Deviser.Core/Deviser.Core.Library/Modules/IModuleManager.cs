using Deviser.Core.Data.Entities;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleManager
    {
        PageContent CreatePageContent(PageContent pageContent);
        PageModule CreatePageModule(PageModule pageModule);
    }
}