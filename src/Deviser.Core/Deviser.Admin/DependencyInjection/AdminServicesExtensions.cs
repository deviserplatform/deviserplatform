using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Deviser.Admin.Config;
using Microsoft.AspNetCore.Builder;

namespace Deviser.Admin
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserAdmin(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddSingleton<IAdminConfigStore, AdminConfigStore>();
            serviceCollection.AddSingleton<IAdminSiteProvider, AdminSiteProvider>();            

            return serviceCollection;
        }

        public static IApplicationBuilder UseDeviserAdmin(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var adminSiteProvider = serviceProvider.GetRequiredService<IAdminSiteProvider>();
            adminSiteProvider.RegisterAdminSites();
            return app;
        }
    }
}
