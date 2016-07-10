using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DeviserWI.Controllers.API
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ModuleController : Controller
    {
        private ILogger<LayoutController> logger;

        IModuleProvider moduleProvider;

        public ModuleController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LayoutController>>();
            moduleProvider = container.Resolve<IModuleProvider>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = moduleProvider.Get();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all modules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
