using Deviser.Core.Data.Entities;

namespace Deviser.Core.Library.Sites
{
    public interface IPageManager
    {
        Page GetPageByUrl(string url, string locale);
        bool HasViewPermission(Page page);
        bool HasEditPermission(Page page);
    }
}