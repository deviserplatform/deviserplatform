using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Data;

namespace Deviser.Admin.Config
{
    public interface IAdminGridService<TModel>
        where TModel : class
    {
        Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties);
        Task<TModel> DeleteItem(string itemId);
    }
}
