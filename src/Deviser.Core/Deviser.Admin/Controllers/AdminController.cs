using Deviser.Admin.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Admin.Controllers
{
    public class AdminController<TAdminConfigurator> : Controller
        where TAdminConfigurator : IAdminConfigurator
    {
        //Logger
        private readonly ILogger<AdminController<TAdminConfigurator>> _logger;

        private readonly Type _adminConfiguratorType;
        private readonly IAdminSiteProvider _adminSiteProvider;
        private readonly DbContext _dbContext;
        private readonly IAdminSite _adminSite;

        public AdminController(IServiceProvider serviceProvider)
        {
            _adminConfiguratorType = typeof(TAdminConfigurator);
            _logger = serviceProvider.GetService<ILogger<AdminController<TAdminConfigurator>>>();
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();
            _adminSite = _adminSiteProvider.GetAdminConfig(_adminConfiguratorType);
            if (_adminSite == null)
                throw new InvalidOperationException($"Admin site is not found for type {_adminConfiguratorType}");
        }

        [HttpGet]
        [Route("api/{entity}/meta/list")]
        public IActionResult GetMetaInfo(string entity)
        {
            try
            {
                var type = _adminSite.AdminConfigs.Keys.FirstOrDefault(t => t.Name == entity);
                IAdminConfig adminConfig;
                if (_adminSite.AdminConfigs.TryGetValue(type, out adminConfig))
                {
                    var fields = adminConfig.FieldConfig.Fields;
                    return Ok(fields);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while restarting the application"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
