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
using Deviser.Core.Library.Hubs;
using Deviser.Core.Library.Internal;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Middleware;
using Deviser.Core.Library.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Infrastructure;
using Deviser.Core.Library.IO;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Media;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Deviser.Core.Library.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        //public static IServiceCollection AddDeviserPlatform(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var logger = new LoggerConfiguration()
        //        //.Enrich.FromLogContext()
        //        .MinimumLevel.Debug()
        //        .WriteTo.RollingFile(Path.Combine("./logs", "log-{Date}.txt"))
        //        .CreateLogger();

        //    services.AddLogging(loggingBuilder =>
        //        loggingBuilder.AddSerilog(logger, true));

        //    services.AddSingleton<IInstallationProvider, InstallationProvider>();
        //    services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
        //    services.AddSingleton<IModuleRegistry, ModuleRegistry>();

        //    InternalServiceProvider.Instance.BuildServiceProvider(services);

        //    IInstallationProvider installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();


        //    services.AddDbContext<DeviserDbContext>(
        //           (internalServiceProvider, dbContextOptionBuilder) =>
        //           {
        //               //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
        //               installationProvider.GetDbContextOptionsBuilder<DeviserDbContext>(dbContextOptionBuilder);
        //           });

        //    services.AddIdentity<User, Role>()
        //       .AddEntityFrameworkStores<DeviserDbContext>()
        //       .AddDefaultTokenProviders();

        //    services.AddHttpContextAccessor();


        //    services.AddAutoMapper(typeof(AdminServicesExtensions).Assembly);

        //    services.AddScoped<ViewResultExecutor>();
        //    services.AddScoped<IScopeService, ScopeService>();
        //    services.AddScoped<IImageOptimizer, ImageOptimizer>();


        //    services.AddScoped<IActionInvoker, ActionInvoker>();
        //    services.AddScoped<ITypeActivatorCache, TypeActivatorCache>();
        //    //builder.RegisterType<ModuleInvokerProvider>().As<IModuleInvokerProvider>();
        //    //services.AddScoped<DeviserRouteHandler>();

        //    services.AddTransient<ILayoutRepository, LayoutRepository>();
        //    services.AddTransient<ILayoutTypeRepository, LayoutTypeRepository>();
        //    services.AddTransient<IContentTypeRepository, ContentTypeRepository>();
        //    services.AddTransient<IModuleRepository, ModuleRepository>();
        //    services.AddTransient<IPageContentRepository, PageContentRepository>();
        //    services.AddTransient<IPageRepository, PageRepository>();
        //    services.AddTransient<IRoleRepository, RoleRepository>();
        //    services.AddTransient<IUserRepository, UserRepository>();
        //    services.AddTransient<ILanguageRepository, LanguageRepository>();
        //    services.AddTransient<IOptionListRepository, OptionListRepository>();
        //    services.AddTransient<IPropertyRepository, PropertyRepository>();

        //    //builder.RegisterType<ContactProvider>().As<IContactProvider>();
        //    services.AddScoped<IRouteConstraint, PageUrlConstraint>();
        //    services.AddScoped<IDeviserControllerFactory, DeviserControllerFactory>();

        //    services.AddScoped<IPageManager, PageManager>();
        //    services.AddScoped<IModuleManager, ModuleManager>();
        //    services.AddScoped<IContentManager, ContentManager>();
        //    services.AddScoped<ILayoutManager, LayoutManager>();
        //    services.AddScoped<IThemeManager, ThemeManager>();
        //    services.AddScoped<INavigation, Navigation>();
        //    services.AddScoped<IFileManagement, FileManagement>();
        //    services.AddScoped<ILanguageManager, LanguageManager>();
        //    services.AddScoped<ISettingManager, SettingManager>();
        //    services.AddScoped<ISitemapService, SitemapService>();

        //    InternalServiceProvider.Instance.BuildServiceProvider(services);

        //    //Add framework services.
        //    //services.AddDbContext<DeviserDbContext>(options =>
        //    //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Deviser.WI")));


        //    if (installationProvider.IsPlatformInstalled)
        //    {


        //        var siteSettingRepository = InternalServiceProvider.Instance.ServiceProvider.GetService<ISiteSettingRepository>(); //sp.GetService<ISiteSettingRepository>();
        //        var siteSettings = siteSettingRepository.GetSettings();

        //        var enableFacebookAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableFacebookAuth").SettingValue;
        //        var facebookAppId = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppId").SettingValue;
        //        var facebookAppAppSecret = siteSettings.FirstOrDefault(s => s.SettingName == "FacebookAppSecret").SettingValue;
        //        var enableGoogleAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableGoogleAuth").SettingValue;
        //        var googleClientId = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientId").SettingValue;
        //        var googleClientSecret = siteSettings.FirstOrDefault(s => s.SettingName == "GoogleClientSecret").SettingValue;
        //        var enableTwitterAuth = siteSettings.FirstOrDefault(s => s.SettingName == "EnableTwitterAuth").SettingValue;
        //        var twitterConsumerKey = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerKey").SettingValue;
        //        var twitterConsumerSecret = siteSettings.FirstOrDefault(s => s.SettingName == "TwitterConsumerSecret").SettingValue;

        //        if (!string.IsNullOrEmpty(enableFacebookAuth) && bool.Parse(enableFacebookAuth.ToLower()))
        //        {
        //            services.AddAuthentication().AddFacebook(facebookOptions =>
        //            {
        //                facebookOptions.AppId = facebookAppId;
        //                facebookOptions.AppSecret = facebookAppAppSecret;
        //            });
        //        }

        //        if (!string.IsNullOrEmpty(enableTwitterAuth) && bool.Parse(enableTwitterAuth.ToLower()))
        //        {
        //            services.AddAuthentication().AddTwitter(twitterOptions =>
        //            {
        //                twitterOptions.ConsumerKey = twitterConsumerKey;
        //                twitterOptions.ConsumerSecret = twitterConsumerSecret;
        //            });
        //        }

        //        if (!string.IsNullOrEmpty(enableGoogleAuth) && bool.Parse(enableGoogleAuth.ToLower()))
        //        {
        //            services.AddAuthentication().AddGoogle(googleOptions =>
        //            {
        //                googleOptions.ClientId = googleClientId;
        //                googleOptions.ClientSecret = googleClientSecret;
        //            });
        //        }
        //    }

        //    RegisterModuleDependencies(services);

            

        //    //services
        //    //    .AddMvc()
        //    //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
        //    //    .AddRazorOptions(options =>
        //    //    {
        //    //        options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
        //    //    })
        //    //    .AddControllersAsServices();

        //    services
        //        .AddControllersWithViews()
        //        .AddNewtonsoftJson(options =>
        //        {
        //            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //            options.SerializerSettings.Formatting = Formatting.Indented;
        //            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //        })
        //        .AddRazorOptions(options =>
        //        {
        //            options.ViewLocationExpanders.Add(new ModuleLocationRemapper());

        //        })
        //        .AddControllersAsServices();
            

        //    services.AddDeviserAdmin();

        //    services.AddSignalR();

        //    services.AddDistributedMemoryCache();

        //    services.AddSession(options =>
        //    {
        //        // Set a short timeout for easy testing.
        //        options.IdleTimeout = TimeSpan.FromSeconds(10);
        //        options.Cookie.HttpOnly = true;
        //        // Make the session cookie essential
        //        options.Cookie.IsEssential = true;
        //    });

        //    services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        //    {
        //        builder.AllowAnyOrigin()
        //               .AllowAnyMethod()
        //               .AllowAnyHeader();
        //    }));

        //    // Add core application services.
        //    services.AddTransient<IEmailSender, MessageSender>();
        //    services.AddTransient<ISmsSender, MessageSender>();
        //    services.TryAddSingleton<ObjectMethodExecutorCache>();
        //    services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();

        //    return services;
        //}

        public static IApplicationBuilder UseDeviserPlatform(this IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {
            //var defaultRequestCulture = new RequestCulture(new CultureInfo(enUSCulture));
            
            IWebHostEnvironment env = serviceProvider.GetService<IWebHostEnvironment>();
            var installationProvider = serviceProvider.GetService<IInstallationProvider>();
            
            if (installationProvider.IsPlatformInstalled)
            {
                var languageRepository = serviceProvider.GetService<ILanguageRepository>();
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var cultureQuery = context.Request.Query["culture"];
                if (!string.IsNullOrWhiteSpace(cultureQuery))
                {
                    var culture = new CultureInfo(cultureQuery);

                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }

                // Call the next delegate/middleware in the pipeline
                await next();
            });

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            //Action<IRouteBuilder> routeBuilder = routes =>
            //{
            //    routes.MapRoute(name: Globals.moduleRoute,
            //        template: "modules/{area:exists}/{controller=Home}/{action=Index}");

            //    routes.MapRoute(
            //       name: "default",
            //       template: "{controller=Page}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //    name: "CmsRoute",
            //    template: "{*permalink}",
            //    defaults: new { controller = "Page", action = "Index" },
            //    constraints: new { permalink = serviceProvider.GetService<IRouteConstraint>() });
            //};

            //Deviser Specific
            app.UsePageContext();
            //app.UseDeviserAdmin(serviceProvider);
            
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ApplicationHub>("/appHub");

                endpoints.MapControllerRoute(name: Globals.moduleRoute,
                    pattern: "modules/{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Page}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "CmsRoute",
                    pattern: "{**permalink}",
                    defaults: new { controller = "Page", action = "Index" },
                    constraints: new { permalink = serviceProvider.GetService<IRouteConstraint>() });
            });
        }

        //private static void RegisterModuleDependencies(IServiceCollection serviceCollection)
        //{
        //    try
        //    {
        //        var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
        //        Type moduleConfiguratorType = typeof(IModuleConfigurator);
        //        List<TypeInfo> moduleConfigurators = assemblies.GetDerivedTypeInfos(moduleConfiguratorType); //new List<TypeInfo>();
        //        var moduleRegistery = InternalServiceProvider.Instance.ServiceProvider.GetService<IModuleRegistry>();
        //        var registerServiceMethodInfo = moduleConfiguratorType.GetMethod("ConfigureServices");


        //        if (moduleConfigurators.Count > 0)
        //        {
        //            foreach (var moduleConfigurator in moduleConfigurators)
        //            {
        //                var moduleManifest = new ModuleManifest();
        //                moduleManifest.ModuleMetaInfo.ModuleAssemblyFullName = moduleConfigurator.Assembly.FullName;
        //                var moduleConfig = Activator.CreateInstance(moduleConfigurator) as IModuleConfigurator;
        //                //registerServiceMethodInfo.Invoke(obj, new object[] { serviceCollection });
        //                moduleConfig.ConfigureModule(moduleManifest);
        //                moduleManifest.ModuleMetaInfo.AdminConfiguratorTypeInfo = GetAdminConfiguratorInModule(moduleConfigurator.Assembly);
        //                moduleRegistery.TryRegisterModule(moduleManifest.ModuleMetaInfo);

        //                moduleConfig.ConfigureServices(serviceCollection);

        //                //RegisterModuleDbContexts
        //                RegisterModuleDbContexts(moduleConfigurator.Assembly, serviceCollection);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        //private static void RegisterModuleDbContexts(Assembly moduleAssemlby, IServiceCollection serviceCollection)
        //{
        //    try
        //    {
        //        //var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
        //        List<TypeInfo> moduleDbContextDerivedTypes = moduleAssemlby.GetDerivedTypeInfos(typeof(ModuleDbContext));
        //        var installationProvider = InternalServiceProvider.Instance.ServiceProvider.GetRequiredService<IInstallationProvider>();
        //        if (moduleDbContextDerivedTypes.Count > 0)
        //        {
        //            foreach (var moduleDbContextType in moduleDbContextDerivedTypes)
        //            {
        //                //MethodInfo addDbContextMethodInfo = GetAddDbContextMethodInfo();

        //                //var modDbContextMethodInfo = addDbContextMethodInfo.MakeGenericMethod(moduleDbContextType);
        //                var moduleAssembly = moduleDbContextType.Assembly.GetName().Name;
        //                //var dbContextOptionExprFactory = new DbContextOptionExprFactory(moduleAssembly, installationProvider);

        //                ////if (moduleAssembly == "Deviser.Modules.Blog")
        //                ////{
        //                //var optionsAction = dbContextOptionExprFactory.GetActionExpression(); //GetActionExpression(moduleAssembly);
        //                //modDbContextMethodInfo.Invoke(serviceCollection, new object[] { serviceCollection, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
        //                ////}

        //                CallGenericMethod(nameof(RegisterModuleDbContext), moduleDbContextType,
        //                    new object[] { moduleAssembly, installationProvider, serviceCollection });

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public static TypeInfo GetAdminConfiguratorInModule(Assembly assembly)
        //{
        //    List<TypeInfo> adminConfiguratorTypes = assembly.GetDerivedTypeInfos(typeof(IAdminConfigurator));
        //    if (adminConfiguratorTypes.Count > 1)
        //    {
        //        throw new InvalidOperationException($"Module cannot have more than one implementation of {nameof(IAdminConfigurator)}");
        //    }

        //    return adminConfiguratorTypes.FirstOrDefault();
        //}

        ////private static MethodInfo GetAddDbContextMethodInfo()
        ////{
        ////    MethodInfo addDbContextMethodInfo = null;

        ////    var methodInfos = typeof(EntityFrameworkServiceCollectionExtensions)
        ////        .GetMethods().Where(m => m.Name == "AddDbContext" && m.GetGenericArguments().Count() == 1).ToList();

        ////    foreach (var mi in methodInfos)
        ////    {
        ////        var parameters = mi.GetParameters();
        ////        if (parameters[0].ParameterType == typeof(IServiceCollection) && !parameters[0].IsOptional &&
        ////            parameters[1].ParameterType == typeof(Action<IServiceProvider, DbContextOptionsBuilder>) && !parameters[1].IsOptional)
        ////        {
        ////            addDbContextMethodInfo = mi;
        ////            break;
        ////        }
        ////    }

        ////    return addDbContextMethodInfo;
        ////}
        ////private static Action<IServiceProvider, DbContextOptionsBuilder> GetActionExpression(string assembly)
        ////{
        ////    var installationProvider = SharedObjects.ServiceProvider.GetRequiredService<IInstallationProvider>();

        ////    Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = (internalServiceprovider, dbContextOptionBuilder) =>
        ////    {
        ////        installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder, assembly/*"Deviser.Modules.Blog"*/);
        ////    };

        ////    return optionsAction;
        ////}

        //private static void RegisterModuleDbContext<TDbContext>(string moduleAssembly,
        //    IInstallationProvider installationProvider,
        //    IServiceCollection serviceCollection)
        //    where TDbContext : DbContext
        //{
        //    serviceCollection.AddDbContext<TDbContext>(
        //           (internalServiceProvider, dbContextOptionBuilder) =>
        //           {
        //               //dbContextOptionBuilder.UseInternalServiceProvider(sp);                    
        //               installationProvider.GetDbContextOptionsBuilder<TDbContext>(dbContextOptionBuilder, moduleAssembly);
        //           });
        //}

        //private static void CallGenericMethod(string methodName, Type genericType, object[] parmeters)
        //{
        //    var getItemMethodInfo = typeof(AdminServicesExtensions).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
        //    var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericType);
        //    var result = getItemMethod.Invoke(null, parmeters);
        //}
    }
}
