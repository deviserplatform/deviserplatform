using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;

namespace Deviser.Core.Library.Controllers
{
    public interface IModuleInvokerProvider
    {
        int Order { get; }
        IModuleActionInvoker CreateInvoker(ActionContext actionContext);
    }
}
