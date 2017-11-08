using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Common;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Middleware;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.WI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Deviser.Core.Data;
using Deviser.Core.Library.Internal;
using Microsoft.AspNetCore.Localization;

namespace Deviser.WI
{
    public class Startup
    {
        private const string enUSCulture = "en-US";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Log.Logger = new LoggerConfiguration()
                //.Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log", "log-{Date}.txt"))
                .CreateLogger();

            //builder.AddEnvironmentVariables();
            //Configuration = builder.Build();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DeviserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Deviser.WI")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DeviserDbContext>()
                .AddDefaultTokenProviders();

            services.Add(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SerializerSettingsSetup>());

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

            //// Add Autofac
            //var containerBuilder = new ContainerBuilder();
            //containerBuilder.RegisterModule<DefaultModule>();            
            //containerBuilder.Populate(services);
            //ApplicationContainer = containerBuilder.Build();

            //// Create the IServiceProvider based on the container.
            //return new  AutofacServiceProvider(ApplicationContainer);


            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.RegisterModule<DefaultModule>();
            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            //
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.Populate(services);
            //builder.RegisterType<MyType>().As<IMyType>();
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ILifetimeScope container,
            IApplicationLifetime appLifetime
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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

           app.UseMvc(routeBuilder);

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            //appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());

        }
    }
}
