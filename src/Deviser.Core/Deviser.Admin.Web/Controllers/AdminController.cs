using Deviser.Admin.Data;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Deviser.Admin.Web.Controllers
{
    public class AdminController<TAdminConfigurator> : Controller
       where TAdminConfigurator : IAdminConfigurator
    {
        //Logger
        private readonly ILogger<AdminController<TAdminConfigurator>> _logger;
        private readonly IAdminRepository<TAdminConfigurator> _adminRepository;


        public AdminController(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<AdminController<TAdminConfigurator>>>();
            _adminRepository = new AdminRepository<TAdminConfigurator>(serviceProvider);
        }

        [Route("modules/[area]/admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/meta")]
        public IActionResult GetMetaInfo(string entity)
        {
            try
            {
                var adminConfig = _adminRepository.GetAdminConfig(entity);
                if (adminConfig != null)
                {                    
                    return Ok(adminConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/meta/list")]
        public IActionResult GetListMetaInfo(string entity)
        {
            try
            {
                var adminConfig = _adminRepository.GetAdminConfig(entity);
                if (adminConfig != null)
                {
                    var listConfig = adminConfig.ListConfig;
                    return Ok(listConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/meta/fields")]
        public IActionResult GetFieldMetaInfo(string entity)
        {
            try
            {
                var adminConfig = _adminRepository.GetAdminConfig(entity);
                if (adminConfig != null)
                {
                    var fieldConfig = new { adminConfig.FieldConfig, adminConfig.FieldSetConfig };
                    return Ok(fieldConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: All records
        [HttpGet]
        [Route("modules/[area]/api/{entity:required}")]
        public IActionResult GetAllRecords(string entity, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            try
            {
                var result = _adminRepository.GetAllFor(entity, pageNo, pageSize, orderBy);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting all records for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: All records
        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/{id:required}")]
        public IActionResult GetItem(string entity, string id)
        {
            try
            {
                var result = _adminRepository.GetItemFor(entity, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting a record for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("modules/[area]/api/{entity:required}")]
        public async Task<IActionResult> Create(string entity, [FromBody]object entityObject)
        {
            try
            {
                var result = _adminRepository.CreateItemFor(entity, entityObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while creating a record for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{entity:required}")]
        public async Task<IActionResult> Update(string entity, [FromBody]object entityObject)
        {
            try
            {
                var result = _adminRepository.UpdateItemFor(entity, entityObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while updating a record for entity: {entity}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("modules/[area]/api/{entity:required}/{id:required}")]
        public async Task<IActionResult> Delete(string entity, string id)
        {
            try
            {
                var result = _adminRepository.DeleteItemFor(entity, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while deleting a record for entity: {entity}, id:{id}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
