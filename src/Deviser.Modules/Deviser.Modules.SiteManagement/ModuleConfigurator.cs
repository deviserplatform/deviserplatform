using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Deviser.Core.Common.Module;
using Deviser.Core.Library.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.SiteManagement
{
    public class ModuleConfigurator : IModuleConfigurator
    {
        public void ConfigureModule(IModuleManifest moduleManifest)
        {
            moduleManifest.ModuleMetaInfo.ModuleName = "SiteManagement";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
