using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Sites
{
    public interface IPageManager
    {
        Page GetPageAndDependencies(Guid pageId);
        Page GetPageAndDependenciesByUrl(string url, string locale);
        bool HasViewPermission(Page page);
        bool HasEditPermission(Page page);
    }
}