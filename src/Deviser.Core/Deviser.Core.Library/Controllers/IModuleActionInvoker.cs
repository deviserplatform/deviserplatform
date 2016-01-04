using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Controllers
{
    public interface IModuleActionInvoker
    {
        Task InvokeAsync();
        Task<IActionResult> InvokeAction();
    }
}
