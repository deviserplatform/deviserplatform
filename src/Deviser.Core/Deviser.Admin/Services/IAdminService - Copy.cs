using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;

namespace Deviser.Admin.Services
{
    public interface IAdminServiceEntity<TModel> //: IAdminService
        where TModel : class
    {
        public Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null, ICollection<string> includeStrings = null);
        public Task<TModel> GetItem(string itemId, ICollection<string> includeStrings = null);
        public Task<IFormResult<TModel>> CreateItem(TModel item);
        public Task<IFormResult<TModel>> UpdateItem(TModel item);
        public Task<IAdminResult<TModel>> DeleteItem(string itemId);
    }
}
