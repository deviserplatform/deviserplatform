using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Modules.Blog.DTO;

namespace Deviser.Modules.Blog.Services
{
    public interface ICategoryService : IAdminService<Category>
    {
        Task<PagedResult<Category>> GetAll(int pageNo, int pageSize, string orderByProperties,
            FilterNode filter = null);

        Task<Category> GetItem(string itemId);
        Task<IFormResult<Category>> CreateItem(Category item);
        Task<IFormResult<Category>> UpdateItem(Category item);
        Task<IAdminResult<Category>> DeleteItem(string itemId);
        ICollection<Category> GetCategories();
    }
}