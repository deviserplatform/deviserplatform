using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory
    {
        Task<string> ExecuteModuleController(ActionContext actionContext, string moduleName, int pageModuleId, string controllerName, string actionName);
        Task<Dictionary<string, string>> GetPageModuleResults(ActionContext actionContext, int pageId);
    }
}