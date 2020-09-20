using Deviser.Core.Library.Layouts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Deviser.Core.Common.Security;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    [PermissionAuthorize("PAGE", "EDIT")]
    public class ThemeController : Controller
    {
        //Logger
        private readonly ILogger<ThemeController> _logger;
        private readonly IThemeManager _themeManager;

        public ThemeController(ILogger<ThemeController> logger,
        IThemeManager themeManager)
        {
            _logger = logger;
            _themeManager = themeManager;
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
