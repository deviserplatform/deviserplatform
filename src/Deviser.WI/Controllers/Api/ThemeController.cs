using Autofac;
using Deviser.Core.Data.Repositories;
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
    public class ThemeController : Controller
    {
        //Logger
        private readonly ILogger<ThemeController> _logger;
        private readonly IThemeManager _themeManager;

        public ThemeController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<ThemeController>>();
            _themeManager = container.Resolve<IThemeManager>();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var page = _themeManager.GetHostThemes();
                if (page != null)
                    return Ok(page);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting themes"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
