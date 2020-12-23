using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;

namespace Deviser.Admin.Services
{
    public interface IAdminService<TModel> //: IAdminService
        where TModel : class
    {
        Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null);
        Task<TModel> GetItem(string itemId);
        Task<IFormResult<TModel>> CreateItem(TModel item);
        Task<IFormResult<TModel>> UpdateItem(TModel item);
        Task<IAdminResult<TModel>> DeleteItem(string itemId);
    }
}
