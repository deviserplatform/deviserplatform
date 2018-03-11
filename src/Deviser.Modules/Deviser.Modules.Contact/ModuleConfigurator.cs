using Deviser.Core.Library.Modules;
using Deviser.Modules.ContactForm.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.ContactForm
{
    public class ModuleConfigurator : IModuleConfigurator
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IContactProvider, ContactProvider>();
        }
    }
}
