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

        [Route("modules/[area]/admin/{model:required}")]
        public IActionResult Admin(string model)
        {
            ViewBag.Model = model;
            return View();
        }

        [HttpGet]
        [Route("modules/[area]/api/{model:required}/meta")]
        public IActionResult GetMetaInfo(string model)
        {
            try
            {   
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(model); //_adminRepository.GetAdminConfig(model);
                if (adminConfig != null)
                {
                    return Ok(adminConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("modules/[area]/api/{model:required}/meta/list")]
        public IActionResult GetListMetaInfo(string model)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(model); //_adminRepository.GetAdminConfig(model);
                if (adminConfig != null)
                {
                    var listConfig = adminConfig.ModelConfig.GridConfig;
                    return Ok(listConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("modules/[area]/api/{model:required}/meta/fields")]
        public IActionResult GetFieldMetaInfo(string model)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(model); //_adminRepository.GetAdminConfig(model);
                if (adminConfig != null)
                {
                    var fieldConfig = new { adminConfig.ModelConfig.FormConfig.FieldConfig, adminConfig.ModelConfig.FormConfig.FieldSetConfig };
                    return Ok(fieldConfig);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting meta info for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("modules/[area]/api/{model:required}")]
        public async Task<IActionResult> GetAllRecords(string model, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if(modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.GetAllFor(modelType, pageNo, pageSize, orderBy); //_adminRepository.GetAllFor(model, pageNo, pageSize, orderBy);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting all records for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
                
        [HttpGet]
        [Route("modules/[area]/api/{model:required}/{id:required}")]
        public async Task<IActionResult> GetItem(string model, string id)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.GetItemFor(modelType, id); //_adminRepository.GetItemFor(model, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting a record for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("modules/[area]/api/{model:required}")]
        public async Task<IActionResult> Create(string model, [FromBody]object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.CreateItemFor(modelType, modelObject); //_adminRepository.CreateItemFor(model, modelObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while creating a record for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}")]
        public async Task<IActionResult> Update(string model, [FromBody]object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.UpdateItemFor(modelType, modelObject); //_adminRepository.UpdateItemFor(model, modelObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while updating a record for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/action/{actionName:required}")]
        public async Task<IActionResult> ExecuteMainFormAction(string model, string actionName, [FromBody]object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.ExecuteMainFormAction(modelType, actionName, modelObject); //_adminRepository.UpdateItemFor(model, modelObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while executing custom action {actionName} for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/form/{formName:required}/")]
        public async Task<IActionResult> CustomFormSubmit(string model, string formName, [FromBody]object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetCustomFormModelType(model, formName);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.CustomFormSubmit(model, formName, modelObject); //_adminRepository.UpdateItemFor(model, modelObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while executing custom form submit action for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/form/{formName:required}/action/{actionName:required}")]
        public async Task<IActionResult> ExecuteCustomFormAction(string model, string formName, string actionName, [FromBody]object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.ExecuteCustomFormAction(modelType, formName, actionName, modelObject); //_adminRepository.UpdateItemFor(model, modelObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while executing custom action {actionName} for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("modules/[area]/api/{model:required}/{id:required}")]
        public async Task<IActionResult> Delete(string model, string id)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.DeleteItemFor(modelType, id); //_adminRepository.DeleteItemFor(model, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while deleting a record for model: {model}, id:{id}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
