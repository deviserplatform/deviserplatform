using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Sites
{
    public interface IPageManager
    {
        Page GetPage(Guid pageId);
        Page GetPageByUrl(string url, string locale);
        bool HasViewPermission(Page page);
        bool HasEditPermission(Page page);
    }
}