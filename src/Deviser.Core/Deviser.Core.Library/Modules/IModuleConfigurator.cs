using Deviser.Core.Common.Module;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Library.Modules
{
    public interface IModuleConfigurator
    {
        void ConfigureModule(IModuleManifest moduleManifest);
        void ConfigureServices(IServiceCollection services);
    }
}
