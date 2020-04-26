using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library
{
    public interface INavigation
    {
        Page GetPageTree();
        Page GetPageTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid());
        MenuItem GetMenuItemTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid());
        Page GetPageAndDependencies(Guid pageId);
        IList<Page> GetPages();
        IList<Page> GetPublicPages();
        IList<Page> GetBreadCrumbs(Guid currentPageId);
        Page CreatePage(Page page);
        Page UpdatePageTree(Page page);
        Page UpdatePageAndChildren(Page page);
        bool DeletePage(Guid pageId, bool forceDelete = false);
        string NavigateUrl(string pageId, string locale = null);
        string NavigateUrl(Guid pageId, string locale = null);
        string NavigateUrl(Page page, string locale = null);
        string NavigateAbsoluteUrl(Page page, string locale = null);
    }
}
