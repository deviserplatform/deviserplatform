using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;

namespace Deviser.Modules.Blog.Services
{
    public interface IBlogService : IAdminService<DTO.Blog>
    {
        Task<PagedResult<DTO.Blog>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null);

        Task<DTO.Blog> GetItem(string itemId);
        Task<IFormResult<DTO.Blog>> CreateItem(DTO.Blog item);
        Task<IFormResult<DTO.Blog>> UpdateItem(DTO.Blog item);
        Task<IAdminResult<DTO.Blog>> DeleteItem(string itemId);
        ICollection<DTO.Blog> GetBlogs();
    }
}