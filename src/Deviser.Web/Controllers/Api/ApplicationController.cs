using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Deviser.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Application")]
    [Authorize]
    public class ApplicationController : Controller
    {
        //Logger
        private readonly ILogger<ApplicationController> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ApplicationController(IHostApplicationLifetime hostApplicationLifetime,
            ILogger<ApplicationController> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        [HttpGet]
        [Route("restart/")]
        public async Task<IActionResult> Restart()
        {
            try
            {
                Task.Run(() =>
                {
                    _hostApplicationLifetime.StopApplication();
                });
                var result = Ok(new {status = "Application has been restarted"});
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while restarting the application"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}