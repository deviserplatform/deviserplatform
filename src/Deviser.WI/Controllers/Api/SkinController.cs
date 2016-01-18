﻿using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
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

        public SkinController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<SkinController>>();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var page = SkinHelper.GetHostSkins();
                if (page != null)
                    return Ok(page);
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting skins"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
