using System.Collections.Generic;
using System;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.DomainTypes;

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