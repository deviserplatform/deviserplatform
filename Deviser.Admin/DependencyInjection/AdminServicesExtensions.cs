using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Deviser.Admin.Config;

namespace Deviser.Admin
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserAdmin(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddSingleton<IAdminConfigStore, AdminConfigStore>();
            serviceCollection.AddSingleton<IAdminSiteProvider, AdminSiteProvider>();
            serviceCollection.AddTransient<IAdminSite, AdminSite>();

            return serviceCollection;
        }
    }
}
