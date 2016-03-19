using System;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using System.Collections.Generic;

namespace Deviser.Core.Library
{
    public interface INavigation
    {
        Page GetPageTree(int pageId);
        Page GetPageTree(int currentPageId, SystemPageFilter systemFilter, int parentId = 0);
        List<Page> GetBreadCrumbs(int currentPageId);
        Page UpdatePageTree(Page page);
        Page UpdateSinglePage(Page page);
        bool DeletePage(int pageId, bool forceDelete = false);
    }
}
