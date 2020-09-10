using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace Deviser.Web.Controllers
{
    public class InstallController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IInstallationProvider _installationProvider;
        private readonly IConfiguration _configuration;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public InstallController(IHostApplicationLifetime hostApplicationLifetime,
            IWebHostEnvironment hostingEnvironment, 
            IInstallationProvider installationProvider,
            IConfiguration configuration)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _hostingEnvironment = hostingEnvironment;
            _installationProvider = installationProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (_installationProvider.IsPlatformInstalled)
                return RedirectToAction("Index", "Home");

            var installModel = new InstallModel
            {
                DatabaseProvider = DatabaseProvider.SQLite,
                //ServerName = "(localdb)\\mssqllocaldb",
                IsIntegratedSecurity = true
            };

            return View(installModel);
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult License()
        {
            return View("Terms");
        }

        [HttpPost]
        public IActionResult Index(InstallModel installModel)
        {
            if (_installationProvider.IsPlatformInstalled)
                return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);

            if (ModelState.IsValid)
            {
                string connectionString = _installationProvider.GetConnectionString(installModel);
                string settingFile = Path.Combine(_hostingEnvironment.ContentRootPath, $"appsettings.{_hostingEnvironment.EnvironmentName}.json");

                try
                {
                    _installationProvider.InstallPlatform(installModel);
                    //ApplicationManager.Instance.Restart();
                    Task.Run(() =>
                    {
                        _hostApplicationLifetime.StopApplication();
                    });
                    return Ok();
                }
                catch (SqlException ex)
                {
                    string error = $"Invalid connection to database, kindly check the connection string parameters: {ex.Message}";
                    ModelState.AddModelError("", error);
                    return new ObjectResult(error)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    ModelState.AddModelError("", error);
                    return new ObjectResult(error)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public IActionResult Success()
        {
            return View();
        }
    }    
}