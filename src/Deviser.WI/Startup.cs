using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.ResolveAnything;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.Modules;
using Deviser.WI.Services;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
namespace Deviser.WI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IContainer container = null;

            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<DeviserDBContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"])
                    .MigrationsAssembly("Deviser.WI"));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DeviserDBContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddRazorOptions(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleLocationRemapper());
            });


            //.AddMvcOptions(options =>
            // {
            //     var jsonOutputFormatter = new JsonOutputFormatter();
            //     jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //     //jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            //     jsonOutputFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //     jsonOutputFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //     //options.OutputFormatters.Remove(options.OutputFormatters.FirstOrDefault())
            //     options.OutputFormatters.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
            //     options.OutputFormatters.Insert(0, jsonOutputFormatter);
            //     options.OutputFormatters
            //})

            services.Configure<MvcOptions>(options =>
            {

                var jsonOutputFormatter = new JsonOutputFormatter();
                jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                jsonOutputFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonOutputFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


                var jsonFormatterOld = options.OutputFormatters.FirstOrDefault(formatter => formatter is JsonOutputFormatter);
                if (jsonFormatterOld != null)
                {
                    options.OutputFormatters.Remove(jsonFormatterOld);
                }
                //options.OutputFormatters.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
                options.OutputFormatters.Insert(0, jsonOutputFormatter);

            });

            services.AddCaching(); // Adds a default in-memory implementation of IDistributedCache

            services.AddSession();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            //containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            container = containerBuilder.Build();

            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ILifetimeScope container)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<DeviserDBContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            var defaultRequestCulture = new RequestCulture(new CultureInfo("en-US"));
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-CH"),
                    new CultureInfo("fr-CH"),
                    new CultureInfo("it-CH")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-CH"),
                    new CultureInfo("fr-CH"),
                    new CultureInfo("it-CH")
                }
            };

            app.UseRequestLocalization(requestLocalizationOptions, defaultRequestCulture);

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(name: "areaRoute",
                    template: "modules/{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                name: "CmsRoute",
                template: "{*permalink}",
                defaults: new { controller = "Page", action = "Index" },
                constraints: new { permalink = container.Resolve<IRouteConstraint>() });
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
