using System.Collections.Generic;
using Deviser.Core.Library.DomainTypes;

namespace Deviser.Core.Library.Layouts
{
    public interface ILayoutManager
    {
        PageLayout CreatePageLayout(PageLayout pageLayout);
        List<PageLayout> GetPageLayouts();
        PageLayout UpdatePageLayout(PageLayout pageLayout);
    }
}