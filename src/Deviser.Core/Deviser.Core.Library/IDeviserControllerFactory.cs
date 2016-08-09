using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Deviser.Core.Common.DomainTypes;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;
using PageModule = Deviser.Core.Data.Entities.PageModule;
using Deviser.Core.Data.Entities;

namespace Deviser.Core.Library.Controllers
{
    public interface IDeviserControllerFactory
    {        
        Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext, Guid pageId);
        Task<string> GetModuleEditResult(ActionContext actionContext, PageModule pageModule, Guid moduleActionid);
    }
}