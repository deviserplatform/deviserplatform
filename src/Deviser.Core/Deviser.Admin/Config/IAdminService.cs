using Deviser.Admin.Data;
using System.Threading.Tasks;

namespace Deviser.Admin.Config
{
    //public interface IAdminService
    //{

    //}

    public interface IAdminService<TModel> //: IAdminService
        where TModel : class
    {
        Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties);
        Task<TModel> GetItem(string itemId);
        Task<FormResult<TModel>> CreateItem(TModel item);
        Task<FormResult<TModel>> UpdateItem(TModel item);
        Task<TModel> DeleteItem(string itemId);
    }

    public interface IAdminFormService<TModel>
        where TModel : class
    {
        Task<TModel> GetModel();
        Task<FormResult<TModel>> SaveModel(TModel item);
    }
}
