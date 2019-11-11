using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using Deviser.Core.Library.Infrastructure;
using Deviser.Core.Library.DependencyInjection;
using Deviser.Core.Library.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Deviser.Modules.Blog.Models;
using Deviser.Core.Common.Internal;
using Deviser.Core.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Deviser.Core.Common;
using System.Reflection;
using System.Collections.Generic;
using Deviser.Core.Data.Extension;
using System.Linq;
using System.Globalization;
using Deviser.Core.Common.Extensions;
using Deviser.Modules.ContactForm.Data;

namespace Deviser.WI
{
    public class Startup
    {
        private const string enUSCulture = "en-US";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                //.Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "logs", "log-{Date}.txt"))
                .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

           services.AddDeviserPlatform(Configuration);

            //TODO : Call AddDbContext for all base classes of ModuleDbContext
            //var installationProvider = SharedObjects.ServiceProvider.GetRequiredService<IInstallationProvider>();
            //services.AddDbContext<BlogDbContext>(
            //       (dbContextOptionBuilder) =>
            //       {
            //           installationProvider.GetDbContextOptionsBuilder(dbContextOptionBuilder, "Deviser.Modules.Blog");
            //       });

            //services.AddDbContext

            // Add Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);

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
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseDeviserPlatform(container);

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            var hubContext = container.Resolve<IHubContext<ApplicationHub>>();

            //Call "OnStarted" method on all connected clients using SignalR
            Task.Delay(5000).ContinueWith(t =>
            {
                hubContext.Clients.All.SendAsync("OnStarted", "server").GetAwaiter().GetResult();
            });
        }
    }
}
