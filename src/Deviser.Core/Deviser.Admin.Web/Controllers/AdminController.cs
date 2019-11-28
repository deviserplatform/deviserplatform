using Deviser.Admin.Data;
using Deviser.Admin.Services;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Deviser.Admin.Web.Controllers
{
    public class AdminController<TAdminConfigurator> : Controller, IAdminController
        where TAdminConfigurator : IAdminConfigurator
    {
        //Logger
        private readonly ILogger<AdminController<TAdminConfigurator>> _logger;
        //private readonly IAdminRepository<TAdminConfigurator> _adminRepository;
        private readonly IServiceProvider _serviceProvider;

        private string Area => RouteData.Values["area"] as string;

        public AdminController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<AdminController<TAdminConfigurator>>>();
            //_adminRepository = new AdminRepository<TAdminConfigurator>(serviceProvider);
        }

        [Route("modules/[area]/admin/{entity:required}")]
        public IActionResult Admin(string entity)
        {
            ViewBag.Entity = entity;
            return View();
        }

        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/meta")]
        public IActionResult GetMetaInfo(string entity)
        {
            try
            {   
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(entity); //_adminRepository.GetAdminConfig(entity);
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
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(entity); //_adminRepository.GetAdminConfig(entity);
                if (adminConfig != null)
                {
                    var listConfig = adminConfig.FormConfig.ListConfig;
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
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(entity); //_adminRepository.GetAdminConfig(entity);
                if (adminConfig != null)
                {
                    var fieldConfig = new { adminConfig.FormConfig.FieldConfig, adminConfig.FormConfig.FieldSetConfig };
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

        [HttpGet]
        [Route("modules/[area]/api/{entity:required}")]
        public async Task<IActionResult> GetAllRecords(string entity, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(entity);
                if(modelType == null)
                {
                    return BadRequest($"Entity {entity} not fould");
                }

                var result = await coreAdminService.GetAllFor(modelType, pageNo, pageSize, orderBy); //_adminRepository.GetAllFor(entity, pageNo, pageSize, orderBy);
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
                
        [HttpGet]
        [Route("modules/[area]/api/{entity:required}/{id:required}")]
        public async Task<IActionResult> GetItem(string entity, string id)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(entity);
                if (modelType == null)
                {
                    return BadRequest($"Entity {entity} not fould");
                }

                var result = await coreAdminService.GetItemFor(modelType, id); //_adminRepository.GetItemFor(entity, id);
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
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(entity);
                if (modelType == null)
                {
                    return BadRequest($"Entity {entity} not fould");
                }

                var result = await coreAdminService.CreateItemFor(modelType, entityObject); //_adminRepository.CreateItemFor(entity, entityObject);
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
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(entity);
                if (modelType == null)
                {
                    return BadRequest($"Entity {entity} not fould");
                }

                var result = await coreAdminService.UpdateItemFor(modelType, entityObject); //_adminRepository.UpdateItemFor(entity, entityObject);
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
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(entity);
                if (modelType == null)
                {
                    return BadRequest($"Entity {entity} not fould");
                }

                var result = await coreAdminService.DeleteItemFor(modelType, id); //_adminRepository.DeleteItemFor(entity, id);
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
