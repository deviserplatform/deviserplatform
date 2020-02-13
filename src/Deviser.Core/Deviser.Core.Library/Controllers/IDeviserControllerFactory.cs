using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory: IDisposable
    {        
        Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext);
        //Task<string> GetModuleEditResultAsString(ActionContext actionContext, PageModule pageModule, Guid moduleActionid);
        Task<IHtmlContent> GetModuleEditResult(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId);

        Task<IHtmlContent> GetAdminPageResult(ActionContext actionContext);
    }
}