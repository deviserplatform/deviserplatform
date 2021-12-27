using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;

namespace Deviser.Admin.Services
{
    public interface IAdminGridService<TModel>
        where TModel : class
    {
        Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null);
        Task<IAdminResult<TModel>> DeleteItem(string itemId);
    }
}
