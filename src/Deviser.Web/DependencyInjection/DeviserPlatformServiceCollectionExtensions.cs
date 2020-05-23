using AutoMapper;
using Deviser.Admin;
using Deviser.Admin.Web.DependencyInjection;
using Deviser.Core.Common;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Internal;
using Deviser.Core.Common.Module;
using Deviser.Core.Data;
using Deviser.Core.Data.Entities;
using Deviser.Core.Data.Extension;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.IO;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Media;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Deviser.Core.Data.Cache;
using Deviser.Web.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Deviser.Web.DependencyInjection
{
    public static class DeviserPlatformServiceCollectionExtensions
    {
        public static IServiceCollection AddDeviserPlatform(this IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine("./logs", "log-{Date}.txt"))
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(logger, true));

            services.AddSingleton<IInstallationProvider, InstallationProvider>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
            services.AddSingleton<IModuleRegistry, ModuleRegistry>();

            InternalServiceProvider.Instance.BuildServiceProvider(services);

            IWebHostEnvironment hostEnvironment =
                InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            IInstallationProvider installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();


            services.AddDbContext<DeviserDbContext>(
                   (internalServiceProvider, dbContextOptionBuilder) =>
                   {
                       installationProvider.GetDbContextOptionsBuilder<DeviserDbContext>(dbContextOptionBuilder);
                   });

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<DeviserDbContext>()
               .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();


            services.AddAutoMapper(typeof(DeviserPlatformServiceCollectionExtensions).Assembly);

            services.AddScoped<ViewResultExecutor>();
            services.AddScoped<IScopeService, ScopeService>();
            services.AddScoped<IImageOptimizer, ImageOptimizer>();


            services.AddScoped<IActionInvoker, ActionInvoker>();
            services.AddScoped<ITypeActivatorCache, TypeActivatorCache>();
            //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();

            services.AddSingleton<IDeviserDataCache, DeviserDataCache>();
            services.AddTransient<ILayoutRepository, LayoutRepository>();
            services.AddTransient<ILayoutTypeRepository, LayoutTypeRepository>();
            services.AddTransient<IContentTypeRepository, ContentTypeRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IPageContentRepository, PageContentRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILanguageRepository, LanguageRepository>();
            services.AddTransient<IOptionListRepository, OptionListRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();

            //builder.RegisterType<ContactProvider>().As<IContactProvider>();
            services.AddScoped<IRouteConstraint, PageUrlConstraint>();
            services.AddScoped<IDeviserControllerFactory, DeviserControllerFactory>();

            services.AddScoped<IPageManager, PageManager>();
            services.AddScoped<IModuleManager, ModuleManager>();
            services.AddScoped<IContentManager, ContentManager>();
            services.AddScoped<ILayoutManager, LayoutManager>();
            services.AddScoped<IThemeManager, ThemeManager>();
            services.AddScoped<INavigation, Navigation>();
            services.AddScoped<IFileManagement, FileManagement>();
            services.AddScoped<ILanguageManager, LanguageManager>();
            services.AddScoped<ISettingManager, SettingManager>();
            services.AddScoped<ISitemapService, SitemapService>();
            services.AddScoped<IViewProvider, ViewProvider>();

            InternalServiceProvider.Instance.BuildServiceProvider(services);

            if (installationProvider.IsPlatformInstalled)
            {
                //ISettingManager cannot be used here since most of ISettingManager dependencies are not yet initialized at this point.
                var siteSettingRepository = InternalServiceProvider.Instance.ServiceProvider.GetService<ISiteSettingRepository>();
                var siteSettings = siteSettingRepository.GetSettings();

                var enableFacebookAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableFacebookAuth")?.SettingValue;
                var facebookAppId = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppId")?.SettingValue;
                var facebookAppAppSecret = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppSecret")?.SettingValue;
                var enableGoogleAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableGoogleAuth")?.SettingValue;
                var googleClientId = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientId")?.SettingValue;
                var googleClientSecret = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientSecret")?.SettingValue;
                var enableTwitterAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableTwitterAuth")?.SettingValue;
                var twitterConsumerKey = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerKey")?.SettingValue;
                var twitterConsumerSecret = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerSecret")?.SettingValue;

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

            RegisterModuleDependencies(services);

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleLocationRemapper());

                })
                .AddControllersAsServices()
                .AddRazorRuntimeCompilation();

            if (hostEnvironment.IsDevelopment())
            {
                services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
                {
                    var libraryPath = Path.GetFullPath(
                        Path.Combine(hostEnvironment.ContentRootPath, "..", "Deviser.Web"));
                    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));

                    libraryPath = Path.GetFullPath(
                        Path.Combine(hostEnvironment.ContentRootPath, "..", "Deviser.Core", "Deviser.Admin.Web"));
                    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                });
            }

            services.AddDeviserAdmin();

            services.AddSignalR();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Add core application services.
            services.AddTransient<IEmailSender, MessageSender>();
            services.AddTransient<ISmsSender, MessageSender>();
            services.TryAddSingleton<ObjectMethodExecutorCache>();
            services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();

            return services;
        }

        private static void RegisterModuleDependencies(IServiceCollection serviceCollection)
        {

            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.EntryPointAssembly);

            var moduleConfiguratorType = typeof(IModuleConfigurator);
            var moduleConfigurators = assemblies.GetDerivedTypeInfos(moduleConfiguratorType); //new List<TypeInfo>();
            var moduleRegistry = InternalServiceProvider.Instance.ServiceProvider.GetService<IModuleRegistry>();
            //var registerServiceMethodInfo = moduleConfiguratorType.GetMethod("ConfigureServices");

            if (moduleConfigurators.Count <= 0) return;

            foreach (var moduleConfigurator in moduleConfigurators)
            {
                var moduleManifest = new ModuleManifest();
                moduleManifest.ModuleMetaInfo.ModuleAssemblyFullName = moduleConfigurator.Assembly.FullName;
                if (Activator.CreateInstance(moduleConfigurator) is IModuleConfigurator moduleConfig)
                {
                    moduleConfig.ConfigureModule(moduleManifest);
                    moduleManifest.ModuleMetaInfo.AdminConfiguratorTypeInfo =
                        GetAdminConfiguratorInModule(moduleConfigurator.Assembly);
                    moduleRegistry.TryRegisterModule(moduleManifest.ModuleMetaInfo);
                    moduleConfig.ConfigureServices(serviceCollection);
                }

                //RegisterModuleDbContexts
                RegisterModuleDbContexts(moduleConfigurator.Assembly, serviceCollection);
            }
        }

        private static void RegisterModuleDbContexts(Assembly moduleAssembly, IServiceCollection serviceCollection)
        {
            if (moduleAssembly == null) throw new ArgumentNullException(nameof(moduleAssembly));
            var moduleDbContextDerivedTypes = moduleAssembly.GetDerivedTypeInfos(typeof(ModuleDbContext));
            var installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();

            if (moduleDbContextDerivedTypes.Count <= 0) return;

            foreach (var moduleDbContextType in moduleDbContextDerivedTypes)
            {
                var assembly = moduleDbContextType.Assembly.GetName().Name;
                CallGenericMethod(nameof(RegisterModuleDbContext), moduleDbContextType,
                    new object[] { assembly, installationProvider, serviceCollection });
            }
        }

        public static TypeInfo GetAdminConfiguratorInModule(Assembly assembly)
        {
            var adminConfiguratorTypes = assembly.GetDerivedTypeInfos(typeof(IAdminConfigurator));
            if (adminConfiguratorTypes.Count > 1)
            {
                throw new InvalidOperationException($"Module cannot have more than one implementation of {nameof(IAdminConfigurator)}");
            }
            return adminConfiguratorTypes.FirstOrDefault();
        }


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

        private static void CallGenericMethod(string methodName, Type genericType, object[] parameters)
        {
            var getItemMethodInfo = typeof(DeviserPlatformServiceCollectionExtensions).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getItemMethodInfo == null) return;

            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericType);
            var result = getItemMethod.Invoke(null, parameters);
        }
    }
}
