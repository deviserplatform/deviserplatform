using System.Collections.Generic;
using Deviser.Core.Library.DomainTypes;
using System;

namespace Deviser.Core.Library.Layouts
{
    public interface ILayoutManager
    {
        List<PageLayout> GetPageLayouts();
        PageLayout GetPageLayout(Guid layoutId);
        PageLayout CreatePageLayout(PageLayout pageLayout);        
        PageLayout UpdatePageLayout(PageLayout pageLayout);
    }
}