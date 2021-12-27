using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Deviser.Core.Common.Hubs;
using Deviser.Core.Library.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Deviser.Web.Controllers
{
    public class InstallController : DeviserController
    {
        private readonly ILogger<InstallController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<ApplicationHub> _hubContext;
        private readonly IInstallationProvider _installationProvider;
        private readonly IConfiguration _configuration;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public InstallController(IHostApplicationLifetime hostApplicationLifetime,
            IHubContext<ApplicationHub> hubContext,
            IWebHostEnvironment hostingEnvironment, 
            IInstallationProvider installationProvider,
            ILogger<InstallController> logger,
            IConfiguration configuration)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _hubContext = hubContext;
            _hostingEnvironment = hostingEnvironment;
            _installationProvider = installationProvider;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            if (_installationProvider.IsPlatformInstalled)
                return RedirectToAction("Index", "Home");

            var installModel = new InstallModel
            {
                DatabaseProvider = DatabaseProvider.SqlLite,
                //ServerName = "(localdb)\\mssqllocaldb",
                IsIntegratedSecurity = true
            };

            return await ViewAsync(installModel);
        }

        public async Task<IActionResult> Terms()
        {
            return await ViewAsync();
        }

        public async Task<IActionResult> License()
        {
            return await ViewAsync("Terms");
        }

        [HttpPost]
        public async Task<IActionResult> Index(InstallModel installModel)
        {
            if (_installationProvider.IsPlatformInstalled)
                return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);

            if (!ModelState.IsValid) return new StatusCodeResult(StatusCodes.Status400BadRequest);

            var connectionString = _installationProvider.GetConnectionString(installModel);
            var settingFile = Path.Combine(_hostingEnvironment.ContentRootPath, $"appsettings.{_hostingEnvironment.EnvironmentName}.json");

            try
            {
                await _installationProvider.InstallPlatform(installModel);
                //ApplicationManager.Instance.Restart();
                await _hubContext.Clients.All.SendAsync("OnUpdateInstallLog", "Restarting the website");
                await Task.Run(() =>
                {
                    _hostApplicationLifetime.StopApplication();
                });
                return Ok();
            }
            catch (SqlException ex)
            {
                var errorMessage = $"Invalid connection to database, kindly check the connection string parameters: {ex.Message}";
                ModelState.AddModelError("", errorMessage);
                _logger.LogError(ex, errorMessage);
                return new ObjectResult(errorMessage)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                ModelState.AddModelError("", errorMessage);
                _logger.LogError(ex, errorMessage);
                return new ObjectResult(errorMessage)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public async Task<IActionResult> Success()
        {
            return await ViewAsync();
        }
    }    
}