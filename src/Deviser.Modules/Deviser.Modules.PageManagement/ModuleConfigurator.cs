using AutoMapper;
using Deviser.Core.Common.Module;
using Deviser.Core.Library.Modules;
using Deviser.Modules.PageManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Deviser.Modules.PageManagement
{
    public class ModuleConfigurator : IModuleConfigurator
    {
        public void ConfigureModule(IModuleManifest moduleManifest)
        {
            moduleManifest.ModuleMetaInfo.ModuleName = "PageManagement";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PageManagementAdminService>();
        }
    }
}
