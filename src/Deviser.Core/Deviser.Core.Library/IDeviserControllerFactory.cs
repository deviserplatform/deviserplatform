using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Deviser.Core.Common.DomainTypes;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory
    {
        Task<string> ExecuteModuleController(ActionContext actionContext, string moduleName, Guid pageModuleId, string controllerName, string actionName);
        Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext, Guid pageId);
    }
}