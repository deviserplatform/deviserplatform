using Deviser.Core.Library.DependencyInjection;
using Deviser.Core.Library.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Deviser.WI
{
    public class Startup
    {
        private const string enUSCulture = "en-US";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDeviserPlatform(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            
            app.UseDeviserPlatform(serviceProvider);

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            //appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());

            //var hubContext = serviceProvider.GetService<IHubContext<ApplicationHub>>();

            ////Call "OnStarted" method on all connected clients using SignalR
            //Task.Delay(5000).ContinueWith(t =>
            //{
            //    hubContext.Clients.All.SendAsync("OnStarted", "server").GetAwaiter().GetResult();
            //});
        }
    }
}
