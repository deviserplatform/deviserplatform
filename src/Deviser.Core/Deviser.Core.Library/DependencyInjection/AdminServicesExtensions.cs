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
using Deviser.Core.Data.DataProviders;
using System.Reflection;
using System.Linq;
using Deviser.Core.Data.Extension;
using Deviser.Core.Common.Internal;

namespace Deviser.Core.Library.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserPlatform(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IInstallationProvider, InstallationProvider>();

            var sp = services.BuildServiceProvider();
            IInstallationProvider installationProvider = sp.GetRequiredService<IInstallationProvider>();

            SharedObjects.ServiceProvider = sp;

            // Add framework services.            
            //services.AddDbContext<DeviserDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Deviser.WI")));

            services.AddDbContext<DeviserDbContext>(
                (internalServiceProvider, dbContextOptionBuilder) =>
                {
                    //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
                    installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder);
                });

            //services.AddDbContext<ModuleDbContext>(
            //    (internalServiceProvider, dbContextOptionBuilder) =>
            //    {
            //        //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
            //        installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder);
            //    });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DeviserDbContext>()
                .AddDefaultTokenProviders();

            services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());

            RegisterModuleDependencies(services);

            services.AddMvc(option =>
            {
                //var jsonOutputFormatter = new JsonOutputFormatter();
                //jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ////jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                //jsonOutputFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //jsonOutputFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                //var jsonOutputFormatterOld = option.OutputFormatters.FirstOrDefault(formatter => formatter is JsonOutputFormatter);
                //if (jsonOutputFormatterOld != null)
                //{
                //    option.OutputFormatters.Remove(jsonOutputFormatterOld);
                //}
                ////options.OutputFormatters.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
                //option.OutputFormatters.Insert(0, jsonOutputFormatter);                
            })
            .AddRazorOptions(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
            })
            .AddControllersAsServices();

            services.AddSession();

            MapperConfig.CreateMaps();

            // Add application services.
            services.AddTransient<IEmailSender, MessageSender>();
            services.AddTransient<ISmsSender, MessageSender>();
            services.AddScoped<IScopeService, ScopeService>();
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
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "default",
                   template: "{controller=Page}/{action=Index}/{id?}");

                routes.MapRoute(name: Globals.moduleRoute,
                    template: "modules/{area:exists}/{controller=Home}/{action=Index}");

                //routes.MapRoute(
                //name: "CmsRouteMultilingual",
                //template: "{culture}/{*permalink}",
                //defaults: new { controller = "Page", action = "Index" },
                //constraints: new { permalink = container.Resolve<IRouteConstraint>() });

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
