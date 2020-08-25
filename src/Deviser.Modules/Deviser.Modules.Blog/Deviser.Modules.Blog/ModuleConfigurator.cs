using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.Module;
using Deviser.Core.Library.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Deviser.Modules.Blog.Services;

namespace Deviser.Modules.Blog
{
    public class ModuleConfigurator : IModuleConfigurator
    {       
        public void ConfigureModule(IModuleManifest moduleManifest)
        {
            moduleManifest.ModuleMetaInfo.ModuleName = "Blog";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<BlogService>();
        }
    }
}
