﻿using System;
using Deviser.Core.Data.Entities;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

using Page = Deviser.Core.Data.Entities.Page;

namespace Deviser.Core.Library
{
    public interface INavigation
    {
        Page GetPageTree();
        Page GetPageTree(Guid pageId);
        Page GetPageTree(Guid currentPageId, SystemPageFilter systemFilter, Guid parentId = new Guid());
        Page GetPage(Guid pageId);
        List<Page> GetPages();
        List<Page> GetBreadCrumbs(Guid currentPageId);
        Page CreatePage(Page page);
        Page UpdatePageTree(Page page);
        Page UpdateSinglePage(Page page);
        bool DeletePage(Guid pageId, bool forceDelete = false);
        string NavigateUrl(string pageId, string locale = null);
        string NavigateUrl(Guid pageId, string locale = null);
        string NavigateUrl(Page page, string locale = null);
    }
}
