using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
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
    [Route("api/[controller]")]
    public class SkinController : Controller
    {
        //Logger
        private readonly ILogger<SkinController> logger;
        private ISkinManager skinManager;

        public SkinController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<SkinController>>();
            skinManager = container.Resolve<ISkinManager>();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var page = skinManager.GetHostSkins();
                if (page != null)
                    return Ok(page);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting skins"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
