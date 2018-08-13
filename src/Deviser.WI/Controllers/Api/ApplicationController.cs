using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Deviser.WI.Program;

namespace Deviser.WI.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Application")]
    [Authorize]
    public class ApplicationController : Controller
    {
        //Logger
        private readonly ILogger<ApplicationController> _logger;
        public ApplicationController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<ApplicationController>>();
        }

        [HttpGet]
        [Route("restart/")]
        public async Task<IActionResult> Restart()
        {
            try
            {
                ApplicationManager.Instance.Restart();
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