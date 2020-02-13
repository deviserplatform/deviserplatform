using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Deviser.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Application")]
    [Authorize]
    public class ApplicationController : Controller
    {
        //Logger
        private readonly ILogger<ApplicationController> _logger;
        public ApplicationController(ILogger<ApplicationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("restart/")]
        public async Task<IActionResult> Restart()
        {
            try
            {
                //ApplicationManager.Instance.Restart();
                return Ok(new { status = "Application has been restarted" });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while restarting the application"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}