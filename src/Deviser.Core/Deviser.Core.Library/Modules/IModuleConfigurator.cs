using Deviser.Core.Common.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleConfigurator
    {
        void ConfigureModule(IModuleManifest moduleManifest);
        void ConfigureServices(IServiceCollection services);
    }
}
