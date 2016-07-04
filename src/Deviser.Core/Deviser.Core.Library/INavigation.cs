using System;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using System.Collections.Generic;

namespace Deviser.Core.Library
{
    public interface INavigation
    {
        Page GetPageTree(Guid pageId);
        Page GetPageTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid());
        List<Page> GetBreadCrumbs(Guid currentPageId);
        Page UpdatePageTree(Page page);
        Page UpdateSinglePage(Page page);
        bool DeletePage(Guid pageId, bool forceDelete = false);
    }
}
