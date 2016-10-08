using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Deviser.Core.Data.DataProviders;
using Autofac;
using Microsoft.AspNetCore.Http;
using Deviser.Core.Common.DomainTypes;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class SiteSettingController : Controller
    {
        private readonly ILogger<SiteSettingController> logger;

        ISiteSettingProvider siteSettingProvider;
        
        public SiteSettingController(ILifetimeScope container)
        {
            siteSettingProvider = container.Resolve<ISiteSettingProvider>();
            logger = container.Resolve<ILogger<SiteSettingController>>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = siteSettingProvider.GetSettings();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all site settings"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]List<SiteSetting> settings)
        {
            try
            {
                if(settings != null)
                {
                    var result = siteSettingProvider.UpdateSetting(settings);
                    if (result != null)
                    return Ok(result);
                }                
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: "), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
