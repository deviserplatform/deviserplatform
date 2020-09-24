using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Deviser.Core.Library.Modules;
using Deviser.Core.Common.Module;
using Deviser.Modules.SecurityRoles.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.SecurityRoles
{
    public class ModuleConfigurator:IModuleConfigurator
    {
        public void ConfigureModule(IModuleManifest moduleManifest)
        {
            moduleManifest.ModuleMetaInfo.ModuleName = "SecurityRoles";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SecurityRoleAdminService>();
        }
    }
}
