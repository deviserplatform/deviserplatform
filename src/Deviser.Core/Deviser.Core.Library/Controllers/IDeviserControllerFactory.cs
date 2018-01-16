using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Deviser.Core.Common.DomainTypes;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;
using Microsoft.AspNetCore.Html;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory
    {        
        Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext);
        Task<string> GetModuleEditResultAsString(ActionContext actionContext, PageModule pageModule, Guid moduleActionid);
        Task<IHtmlContent> GetModuleEditResult(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId);
    }
}