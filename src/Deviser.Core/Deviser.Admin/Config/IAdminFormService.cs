using System.Threading.Tasks;

namespace Deviser.Admin.Config
{
    public interface IAdminFormService<TModel>
        where TModel : class
    {
        Task<TModel> GetModel();
        Task<FormResult<TModel>> SaveModel(TModel item);
    }
}