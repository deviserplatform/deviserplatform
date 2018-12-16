using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Deviser.Admin.Config;
using Microsoft.AspNetCore.Builder;
using Deviser.Admin.Validation;

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

            services.ConfigureOptions(typeof(UIConfigureOptions));

            return services;
        }

        public static IApplicationBuilder UseDeviserAdmin(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var adminSiteProvider = serviceProvider.GetRequiredService<IAdminSiteProvider>();
            adminSiteProvider.RegisterAdminSites();
            return app;
        }
    }
}
