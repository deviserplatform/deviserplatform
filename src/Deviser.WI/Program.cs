using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Deviser.WI
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    try
        //    {
        //        CreateWebHostBuilder(args).Build().Run();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        public static IWebHost _WebHost;

        public static void Main(string[] args)
        {
            var appManager = ApplicationManager.Instance;

            do
            {
                appManager.Start(args);
            } while (appManager.Restarting);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public class ApplicationManager
        {

            private static ApplicationManager _appManager;
            private IWebHost _web;
            private CancellationTokenSource _tokenSource;
            private bool _running;
            private bool _restart;

            public bool Restarting => _restart;

            private ApplicationManager()
            {
                _running = false;
                _restart = false;

            }

            public static ApplicationManager Instance
            {
                get
                {
                    if (_appManager == null)
                        _appManager = new ApplicationManager();

                    return _appManager;
                }
            }

            public void Start(string[] args)
            {
                if (_running)
                    return;

                if (_tokenSource != null && _tokenSource.IsCancellationRequested)
                    return;

                _tokenSource = new CancellationTokenSource();
                _tokenSource.Token.ThrowIfCancellationRequested();
                _running = true;

                _WebHost = CreateWebHostBuilder(args).Build();
                _WebHost.RunAsync(_tokenSource.Token).GetAwaiter().GetResult();
            }

            public async Task Stop()
            {
                if (!_running)
                    return;

                _tokenSource.Cancel();
                await _WebHost.WaitForShutdownAsync(_tokenSource.Token);
                _running = false;
            }

            public async Task Restart()
            {
                await Stop();

                _restart = true;
                _tokenSource = null;
            }
        }
    }
}
