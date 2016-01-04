using System;
using Deviser.Core.Data.Entities;

namespace Deviser.Core.Library
{
    public interface INavigation
    {
        Page UpdatePageTree(Page page);
        Page UpdateSinglePage(Page page);
        bool DeletePage(int pageId, bool forceDelete = false);
    }
}
