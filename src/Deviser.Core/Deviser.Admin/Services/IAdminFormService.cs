using System.Threading.Tasks;
using Deviser.Admin.Config;

namespace Deviser.Admin.Services
{
    public interface IAdminFormService<TModel>
        where TModel : class
    {
        Task<TModel> GetModel();
        Task<IFormResult<TModel>> SaveModel(TModel item);
    }
}