using System.Threading.Tasks;

namespace Deviser.Core.Library.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
