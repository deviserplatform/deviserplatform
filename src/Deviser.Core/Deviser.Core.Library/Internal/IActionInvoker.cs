using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deviser.Core.Library.Internal
{
    public interface IActionInvoker
    {
        Task<IActionResult> InvokeAction(HttpContext httpContext, ModuleAction moduleAction, ActionContext actionContext);
    }
}