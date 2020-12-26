using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Modules.Blog.DTO;

namespace Deviser.Modules.Blog.Services
{
    public interface ITagService : IAdminService<Tag>
    {
        Task<PagedResult<Tag>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null);

        Task<Tag> GetItem(string itemId);
        Task<IFormResult<Tag>> CreateItem(Tag item);
        Task<IFormResult<Tag>> UpdateItem(Tag item);
        Task<IAdminResult<Tag>> DeleteItem(string itemId);
        ICollection<Tag> GetTags();
    }
}