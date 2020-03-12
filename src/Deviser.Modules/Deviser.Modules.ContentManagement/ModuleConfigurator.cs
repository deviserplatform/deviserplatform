using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Deviser.Core.Common.Module;
using Deviser.Core.Library.Modules;
using Deviser.Modules.ContentManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.ContentManagement
{
    public class ModuleConfigurator : IModuleConfigurator
    {
        public void ConfigureModule(IModuleManifest moduleManifest)
        {
            moduleManifest.ModuleMetaInfo.ModuleName = "ContentManagement";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ContentTypeAdminService>();
            services.AddScoped<LayoutTypeAdminService>();
            services.AddScoped<PropertyAdminService>();
            services.AddScoped<OptionListAdminService>();
        }
    }
}
