using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.WI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            bool isCancelKeyPressed = false;

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                isCancelKeyPressed = true;
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };

            while (true)
            {
                Console.WriteLine("Application is starting");
                await ApplicationManager.Instance.Start(args);
                Console.WriteLine("Application has been terminated");
                if (isCancelKeyPressed)
                    break;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public class ApplicationManager
    {
        private static ApplicationManager _instance;
        private static IApplicationLifetime _applicationLifetime;
        private ApplicationManager()
        {

        }

        public static ApplicationManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ApplicationManager();
                return _instance;
            }
        }

        public async Task Start(string[] args)
        {
            var webHost = Program.CreateWebHostBuilder(args).Build();
            _applicationLifetime = webHost.Services.GetService<IApplicationLifetime>();
            await webHost.RunAsync();
        }

        public void Stop()
        {
            _applicationLifetime.StopApplication();
        }

        public void Restart()
        {
            Stop();
        }
    }
}
