using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Deviser.WI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            //try
            //{
                //Log.Information("Application host being started...");
                CreateHostBuilder(args).Build().Run();
            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, "Host terminated unexpectedly");
            //}
            //finally
            //{
            //    Log.CloseAndFlush();
            //}
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }


    //public class Program
    //{
    //    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    //        .AddEnvironmentVariables()
    //        .Build();

    //    public static void Main(string[] args)
    //    {
    //        Log.Logger = new LoggerConfiguration()
    //            //.Enrich.FromLogContext()
    //            .MinimumLevel.Debug()
    //            .WriteTo.RollingFile(Path.Combine("./logs", "log-{Date}.txt"))
    //            .CreateLogger();


    //        try
    //        {
    //            Log.Information("Application host being started...");

    //            //bool isCancelKeyPressed = false;

    //            //Console.CancelKeyPress += (sender, eventArgs) =>
    //            //{
    //            //    isCancelKeyPressed = true;
    //            //    // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
    //            //    eventArgs.Cancel = true;
    //            //};

    //            //while (true)
    //            //{
    //            //    Console.WriteLine("Application is starting");
    //            //    ApplicationManager.Instance.Start(args);
    //            //    Console.WriteLine("Application has been terminated");
    //            //    if (isCancelKeyPressed)
    //            //        break;
    //            //}

    //            CreateHostBuilder(args).Build().Run();
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Fatal(ex, "Host terminated unexpectedly");
    //        }
    //        finally
    //        {
    //            Log.CloseAndFlush();
    //        }
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder
    //                    .UseStartup<Startup>();
    //            })
    //            .UseSerilog();
    //}

    //public class ApplicationManager
    //{
    //    private static ApplicationManager _instance;
    //    private static IHostApplicationLifetime _applicationLifetime;
    //    private ApplicationManager()
    //    {

    //    }

    //    public static ApplicationManager Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //                _instance = new ApplicationManager();
    //            return _instance;
    //        }
    //    }

    //    public void Start(string[] args)
    //    {
    //        var webHost = Program.CreateHostBuilder(args).Build();
    //        _applicationLifetime = webHost.Services.GetService<IHostApplicationLifetime>();
    //        webHost.Run();
    //    }

    //    public void Stop()
    //    {
    //        _applicationLifetime.StopApplication();
    //    }

    //    public void Restart()
    //    {
    //        Stop();
    //    }
    //}
}
