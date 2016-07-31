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
        Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext, Guid pageId);
        Task<string> GetModuleEditResult(ActionContext actionContext, Guid pageModuleId, Guid moduleActionid);
    }
}