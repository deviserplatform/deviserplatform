using Deviser.Core.Library.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Deviser.Core.Data;
using Microsoft.EntityFrameworkCore;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Deviser.Core.Library.Infrastructure;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Internal;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Deviser.Core.Library.Middleware;
using Microsoft.AspNetCore.Routing;
using Deviser.Core.Common;
using Autofac;
using Deviser.Core.Data.Repositories;
using System.Reflection;
using System.Linq;
using Deviser.Core.Data.Extension;
using Deviser.Core.Common.Internal;
using AutoMapper;

namespace Deviser.Core.Library.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserPlatform(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IInstallationProvider, InstallationProvider>();
            services.AddSingleton<ISiteSettingRepository, SiteSettingRepository>();

            var sp = services.BuildServiceProvider();
            IInstallationProvider installationProvider = sp.GetRequiredService<IInstallationProvider>();

            SharedObjects.ServiceProvider = sp;


            //if (installationProvider.IsPlatformInstalled)
            //{
                services.AddDbContext<DeviserDbContext>(
                       (internalServiceProvider, dbContextOptionBuilder) =>
                       {
                           //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
                           installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder);
                       });
            //}

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<DeviserDbContext>()
               .AddDefaultTokenProviders();

            MapperConfig.CreateMaps();
            sp = services.BuildServiceProvider();

            //Add framework services.
            //services.AddDbContext<DeviserDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Deviser.WI")));

            
            if (installationProvider.IsPlatformInstalled)
            {
               

                var siteSettingRepository = sp.GetService<ISiteSettingRepository>(); //sp.GetService<ISiteSettingRepository>();
                var siteSettings = siteSettingRepository.GetSettings();

                var enableFacebookAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableFacebookAuth").SettingValue;
                var facebookAppId = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppId").SettingValue;
                var facebookAppAppSecret = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppSecret").SettingValue;
                var enableGoogleAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableGoogleAuth").SettingValue;
                var googleClientId = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientId").SettingValue;
                var googleClientSecret = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientSecret").SettingValue;
                var enableTwitterAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableTwitterAuth").SettingValue;
                var twitterConsumerKey = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerKey").SettingValue;
                var twitterConsumerSecret = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerSecret").SettingValue;

                if (!string.IsNullOrEmpty(enableFacebookAuth) && bool.Parse(enableFacebookAuth.ToLower()))
                {
                    services.AddAuthentication().AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = facebookAppId;
                        facebookOptions.AppSecret = facebookAppAppSecret;
                    });
                }

                if (!string.IsNullOrEmpty(enableTwitterAuth) && bool.Parse(enableTwitterAuth.ToLower()))
                {
                    services.AddAuthentication().AddTwitter(twitterOptions =>
                    {
                        twitterOptions.ConsumerKey = twitterConsumerKey;
                        twitterOptions.ConsumerSecret = twitterConsumerSecret;
                    });
                }

                if (!string.IsNullOrEmpty(enableGoogleAuth) && bool.Parse(enableGoogleAuth.ToLower()))
                {
                    services.AddAuthentication().AddGoogle(googleOptions =>
                    {
                        googleOptions.ClientId = googleClientId;
                        googleOptions.ClientSecret = googleClientSecret;
                    });
                }
            }

            services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());

            RegisterModuleDependencies(services);

            services
                .AddMvc()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
                })
                .AddControllersAsServices();

            services.AddSession();

            // Add application services.
            services.AddTransient<IEmailSender, MessageSender>();
            services.AddTransient<ISmsSender, MessageSender>();
            services.TryAddSingleton<ObjectMethodExecutorCache>();
            services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();

            return services;
        }

        public static IApplicationBuilder UseDeviserPlatform(this IApplicationBuilder app, ILifetimeScope container)
        {
            //var defaultRequestCulture = new RequestCulture(new CultureInfo(enUSCulture));
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("de-CH"),
                new CultureInfo("fr-CH"),
                new CultureInfo("it-CH")
            };

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                //DefaultRequestCulture = defaultRequestCulture,
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseStaticFiles();

            app.UseAuthentication();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            Action<IRouteBuilder> routeBuilder = routes =>
            {
                routes.MapRoute(name: Globals.moduleRoute,
                    template: "modules/{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                   name: "default",
                   template: "{controller=Page}/{action=Index}/{id?}");

                routes.MapRoute(
                name: "CmsRoute",
                template: "{*permalink}",
                defaults: new { controller = "Page", action = "Index" },
                constraints: new { permalink = container.Resolve<IRouteConstraint>() });
            };

            app.UsePageContext(routeBuilder);

            return app.UseMvc(routeBuilder);
        }

        private static void RegisterModuleDependencies(IServiceCollection serviceCollection)
        {
            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
            List<TypeInfo> moduleServiceCollectors = new List<TypeInfo>();

            Type moduleServiceCollectionType = typeof(IModuleConfigurator);
            MethodInfo registerServiceMethodInfo = moduleServiceCollectionType.GetMethod("ConfigureServices");
            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.DefinedTypes.Where(t => moduleServiceCollectionType.IsAssignableFrom(t)).ToList();

                if (controllerTypes != null && controllerTypes.Count > 0)
                    moduleServiceCollectors.AddRange(controllerTypes);
            }

            if (moduleServiceCollectors.Count > 0)
            {
                foreach (var serviceCollectorType in moduleServiceCollectors)
                {
                    var obj = Activator.CreateInstance(serviceCollectorType);
                    registerServiceMethodInfo.Invoke(obj, new object[] { serviceCollection });
                }
            }

        }
    }
}
