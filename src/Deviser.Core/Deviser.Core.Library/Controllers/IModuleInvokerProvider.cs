using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;

namespace Deviser.Core.Library.Controllers
{
    public interface IModuleInvokerProvider
    {
        int Order { get; }
        IActionInvoker CreateInvoker(ActionContext actionContext);
    }
}
