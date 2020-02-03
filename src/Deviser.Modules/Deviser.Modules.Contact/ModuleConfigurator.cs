using Deviser.Core.Common.Module;
using Deviser.Core.Library.Modules;
using Deviser.Modules.ContactForm.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Deviser.Modules.ContactForm
{
    public class ModuleConfigurator : IModuleConfigurator
    {
        public void ConfigureModule(IModuleManifest moduleManifest)
        {            
            moduleManifest.ModuleMetaInfo.ModuleName = "Contact";
            moduleManifest.ModuleMetaInfo.ModuleVersion = typeof(ModuleConfigurator).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IContactProvider, ContactProvider>();
        }
    }
}
