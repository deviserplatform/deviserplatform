using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;

namespace Deviser.Admin.Services
{
    public interface IAdminService<TModel> //: IAdminService
        where TModel : class
    {
        public Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null);
        public Task<TModel> GetItem(string itemId);
        public Task<IFormResult<TModel>> CreateItem(TModel item);
        public Task<IFormResult<TModel>> UpdateItem(TModel item);
        public Task<IAdminResult<TModel>> DeleteItem(string itemId);
    }
}
