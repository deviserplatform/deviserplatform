using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Deviser.Admin.Web.Controllers
{
    public interface IAdminController
    {
        IActionResult Admin(string entity);
        Task<IActionResult> Create(string entity, [FromBody] object entityObject);
        Task<IActionResult> Delete(string entity, string id);
        IActionResult GetAllRecords(string entity, int pageNo = 1, int pageSize = 10, string orderBy = null);
        IActionResult GetFieldMetaInfo(string entity);
        IActionResult GetItem(string entity, string id);
        IActionResult GetListMetaInfo(string entity);
        IActionResult GetMetaInfo(string entity);
        Task<IActionResult> Update(string entity, [FromBody] object entityObject);
    }
}