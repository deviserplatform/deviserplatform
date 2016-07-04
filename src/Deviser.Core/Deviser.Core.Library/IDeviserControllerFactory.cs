using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Deviser.Core.Library.DomainTypes;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory
    {
        Task<string> ExecuteModuleController(ActionContext actionContext, string moduleName, Guid pageModuleId, string controllerName, string actionName);
        Task<Dictionary<string, List<DomainTypes.ContentResult>>> GetPageModuleResults(ActionContext actionContext, Guid pageId);
    }
}