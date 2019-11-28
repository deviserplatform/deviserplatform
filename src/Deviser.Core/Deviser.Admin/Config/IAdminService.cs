using Deviser.Admin.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Admin.Config
{
    public interface IAdminService
    {

    }

    public interface IAdminService<TModel> : IAdminService
        where TModel : class
    {
        Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties);
        Task<TModel> GetItem(string itemId);
        Task<TModel> CreateItem(TModel item);
        Task<TModel> UpdateItem(TModel item);
        Task<TModel> DeleteItem(string itemId);
    }
}
