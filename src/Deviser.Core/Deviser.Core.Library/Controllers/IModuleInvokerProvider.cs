using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Deviser.Core.Library.Controllers
{
    public interface IModuleInvokerProvider
    {
        int Order { get; }
        IModuleActionInvoker CreateInvoker(ActionContext actionContext);
    }
}
