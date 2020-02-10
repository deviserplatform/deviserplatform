using Deviser.Admin.Config;
using Deviser.Admin.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Deviser.Admin.Web.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserAdmin(this IServiceCollection services)
        {

            services.AddSingleton<IAdminConfigStore, AdminConfigStore>();
            services.AddSingleton<IAdminSiteProvider, AdminSiteProvider>();

            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<IUserByEmailValidator, UserValidator>();

            //services.ConfigureOptions(typeof(UIConfigureOptions));

            
            var serviceProvider = services.BuildServiceProvider();
            RegisterAdminSites(serviceProvider);

            return services;
        }

        public static void RegisterAdminSites(IServiceProvider serviceProvider)
        {
            var adminSiteProvider = serviceProvider.GetRequiredService<IAdminSiteProvider>();
            adminSiteProvider.RegisterAdminSites(serviceProvider);
        }
    }
}
