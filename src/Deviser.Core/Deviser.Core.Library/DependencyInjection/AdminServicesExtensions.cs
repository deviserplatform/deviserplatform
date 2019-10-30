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
using Microsoft.AspNetCore.Builder;
using Deviser.Core.Library.Infrastructure;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Internal;
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
using System.Diagnostics;
using Deviser.Core.Library.Hubs;
using Deviser.Admin;
using System.Linq.Expressions;
using Deviser.Core.Common.Extensions;
using Deviser.Admin.Web.DependencyInjection;
using Deviser.Core.Common.Json;
using Deviser.Core.Common.Module;

namespace Deviser.Core.Library.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserPlatform(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IInstallationProvider, InstallationProvider>();
            services.AddSingleton<ISiteSettingRepository, SiteSettingRepository>();
            services.AddSingleton<IModuleRegistry, ModuleRegistry>();

            InternalServiceProvider.Instance.BuildServiceProvider(services);
            
            IInstallationProvider installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();


            services.AddDbContext<DeviserDbContext>(
                   (internalServiceProvider, dbContextOptionBuilder) =>
                   {
                       //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
                       installationProvider.GetDbContextOptionsBuilder<DeviserDbContext>(dbContextOptionBuilder);
                   });

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<DeviserDbContext>()
               .AddDefaultTokenProviders();

            MapperConfig.CreateMaps();
            InternalServiceProvider.Instance.BuildServiceProvider(services);

            //Add framework services.
            //services.AddDbContext<DeviserDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Deviser.WI")));


            if (installationProvider.IsPlatformInstalled)
            {


                var siteSettingRepository = InternalServiceProvider.Instance.ServiceProvider.GetService<ISiteSettingRepository>(); //sp.GetService<ISiteSettingRepository>();
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

            //RegisterModuleDbContexts(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
                })
                .AddControllersAsServices()
                /*.AddJsonOptions(options => {
                    options.SerializerSettings.Converters.Add(new ExpressionJsonConverter());
                })*/;
            services.AddDeviserAdmin();

            services.AddSignalR();

            services.AddSession();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

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

            var installationProvider = container.Resolve<IInstallationProvider>();
            var serviceProvider = container.Resolve<IServiceProvider>();
            if (installationProvider.IsPlatformInstalled)
            {
                var languageRepository = container.Resolve<ILanguageRepository>();
                var activeLangauges = languageRepository.GetActiveLanguages();
                var supportedCultures = activeLangauges.Select(al => new CultureInfo(al.CultureCode)).ToArray();

                var requestLocalizationOptions = new RequestLocalizationOptions
                {
                    //DefaultRequestCulture = defaultRequestCulture,
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };
                app.UseRequestLocalization(requestLocalizationOptions);
            }

            app.UseHttpsRedirection();

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

            app.UseSignalR(routes =>
            {
                routes.MapHub<ApplicationHub>("/appHub");
            });

            app.UseDeviserAdmin(serviceProvider);

            //// Shows UseCors with CorsPolicyBuilder.
            //app.UseCors(builder =>
            //   builder.WithOrigins("http://example.com"));

            app.UseCors("MyPolicy");


            return app.UseMvc(routeBuilder);
        }

        private static void RegisterModuleDependencies(IServiceCollection serviceCollection)
        {
            try
            {
                var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
                Type moduleConfiguratorType = typeof(IModuleConfigurator);
                List<TypeInfo> moduleConfigurators = assemblies.GetDerivedTypeInfos(moduleConfiguratorType); //new List<TypeInfo>();
                var moduleRegistery = InternalServiceProvider.Instance.ServiceProvider.GetService<IModuleRegistry>();
                var registerServiceMethodInfo = moduleConfiguratorType.GetMethod("ConfigureServices");


                if (moduleConfigurators.Count > 0)
                {
                    foreach (var moduleConfigurator in moduleConfigurators)
                    {
                        var moduleManifest = new ModuleManifest();
                        moduleManifest.ModuleMetaInfo.ModuleAssembly = moduleConfigurator.Assembly.FullName;
                        var moduleConfig = Activator.CreateInstance(moduleConfigurator) as IModuleConfigurator;
                        //registerServiceMethodInfo.Invoke(obj, new object[] { serviceCollection });
                        moduleConfig.ConfigureModule(moduleManifest);
                        moduleRegistery.TryRegisterModule(moduleManifest.ModuleMetaInfo);
                        moduleConfig.ConfigureServices(serviceCollection);

                        //RegisterModuleDbContexts
                        RegisterModuleDbContexts(moduleConfigurator.Assembly, serviceCollection);

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private static void RegisterModuleDbContexts(Assembly moduleAssemlby, IServiceCollection serviceCollection)
        {
            try
            {
                //var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
                List<TypeInfo> moduleDbContextDerivedTypes = moduleAssemlby.GetDerivedTypeInfos(typeof(ModuleDbContext));
                var installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();
                if (moduleDbContextDerivedTypes.Count > 0)
                {
                    foreach (var moduleDbContextType in moduleDbContextDerivedTypes)
                    {
                        //MethodInfo addDbContextMethodInfo = GetAddDbContextMethodInfo();

                        //var modDbContextMethodInfo = addDbContextMethodInfo.MakeGenericMethod(moduleDbContextType);
                        var moduleAssembly = moduleDbContextType.Assembly.GetName().Name;
                        //var dbContextOptionExprFactory = new DbContextOptionExprFactory(moduleAssembly, installationProvider);

                        ////if (moduleAssembly == "Deviser.Modules.Blog")
                        ////{
                        //var optionsAction = dbContextOptionExprFactory.GetActionExpression(); //GetActionExpression(moduleAssembly);
                        //modDbContextMethodInfo.Invoke(serviceCollection, new object[] { serviceCollection, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
                        ////}

                        CallGenericMethod(nameof(RegisterModuleDbContext), moduleDbContextType,
                            new object[] { moduleAssembly, installationProvider, serviceCollection });

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        
        //private static MethodInfo GetAddDbContextMethodInfo()
        //{
        //    MethodInfo addDbContextMethodInfo = null;

        //    var methodInfos = typeof(EntityFrameworkServiceCollectionExtensions)
        //        .GetMethods().Where(m => m.Name == "AddDbContext" && m.GetGenericArguments().Count() == 1).ToList();

        //    foreach (var mi in methodInfos)
        //    {
        //        var parameters = mi.GetParameters();
        //        if (parameters[0].ParameterType == typeof(IServiceCollection) && !parameters[0].IsOptional &&
        //            parameters[1].ParameterType == typeof(Action<IServiceProvider, DbContextOptionsBuilder>) && !parameters[1].IsOptional)
        //        {
        //            addDbContextMethodInfo = mi;
        //            break;
        //        }
        //    }

        //    return addDbContextMethodInfo;
        //}
        //private static Action<IServiceProvider, DbContextOptionsBuilder> GetActionExpression(string assembly)
        //{
        //    var installationProvider = SharedObjects.ServiceProvider.GetRequiredService<IInstallationProvider>();

        //    Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = (internalServiceprovider, dbContextOptionBuilder) =>
        //    {
        //        installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder, assembly/*"Deviser.Modules.Blog"*/);
        //    };

        //    return optionsAction;
        //}

        private static void RegisterModuleDbContext<TDbContext>(string moduleAssembly,
            IInstallationProvider installationProvider,
            IServiceCollection serviceCollection)
            where TDbContext : DbContext
        {
            serviceCollection.AddDbContext<TDbContext>(
                   (internalServiceProvider, dbContextOptionBuilder) =>
                   {
                       //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
                       installationProvider.GetDbContextOptionsBuilder<TDbContext>(dbContextOptionBuilder, moduleAssembly);
                   });
        }

        private static void CallGenericMethod(string methodName, Type genericType, object[] parmeters)
        {
            var getItemMethodInfo = typeof(AdminServicesExtensions).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericType);
            var result = getItemMethod.Invoke(null, parmeters);
        }
    }

    internal class DbContextOptionExprFactory
    {
        private readonly string _assembly;
        private readonly IInstallationProvider _installationProvider;
        internal DbContextOptionExprFactory(string assembly, IInstallationProvider installationProvider)
        {
            _assembly = assembly;
            _installationProvider = installationProvider;
        }

        internal Action<IServiceProvider, DbContextOptionsBuilder> GetActionExpression()
        {
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = (internalServiceprovider, dbContextOptionBuilder) =>
            {
                _installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder, _assembly/*"Deviser.Modules.Blog"*/);
            };
            return optionsAction;
        }
    }
}
