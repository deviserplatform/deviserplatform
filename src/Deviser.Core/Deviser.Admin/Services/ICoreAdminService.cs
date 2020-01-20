using Deviser.Admin.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Admin.Services
{
    public interface ICoreAdminService
    {
        Type GetModelType(string strModelType);
        IAdminConfig GetAdminConfig(string entity);

        Task<object> GetAllFor(Type modelType, int pageNo, int pageSize, string orderByProperties);
        Task<object> GetItemFor(Type modelType, string itemId);
        Task<object> CreateItemFor(Type modelType, object item);
        Task<object> UpdateItemFor(Type modelType, object item);
        Task<object> DeleteItemFor(Type modelType, string itemId);
        Task<object> ExecuteMainFormAction(Type modelType, string actionName, object entityObject);
        Task<object> ExecuteCustomFormAction(Type modelType, string formName, string actionName, object entityObject);
    }
}
