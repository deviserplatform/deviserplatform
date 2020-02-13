﻿using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Layouts
{
    public interface ILayoutManager
    {
        List<PageLayout> GetPageLayouts();
        List<Layout> GetDeletedLayouts();
        PageLayout GetPageLayout(Guid layoutId);
        PageLayout CreatePageLayout(PageLayout pageLayout);        
        PageLayout UpdatePageLayout(PageLayout pageLayout);
        Layout UpdateLayout(Layout layout);
    }
}