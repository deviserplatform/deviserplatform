using System.Collections.Generic;
using Deviser.Core.Library.DomainTypes;

namespace Deviser.Core.Library.Layouts
{
    public interface ILayoutManager
    {
        List<PageLayout> GetPageLayouts();
        PageLayout GetPageLayout(int layoutId);
        PageLayout CreatePageLayout(PageLayout pageLayout);        
        PageLayout UpdatePageLayout(PageLayout pageLayout);
    }
}