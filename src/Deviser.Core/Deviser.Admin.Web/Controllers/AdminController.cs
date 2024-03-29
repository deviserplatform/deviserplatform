﻿using AutoMapper;
using Deviser.Admin.Services;
using Deviser.Admin.Validation;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Admin.Config.Filters;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.Security;

namespace Deviser.Admin.Web.Controllers
{
    [PermissionAuthorize("PAGE", "EDIT")]
    public class AdminController<TAdminConfigurator> : Controller, IAdminController
        where TAdminConfigurator : IAdminConfigurator
    {
        //Logger
        private readonly ILogger<AdminController<TAdminConfigurator>> _logger;
        //private readonly IAdminRepository<TAdminConfigurator> _adminRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUserByEmailValidator _userByEmailValidator;

        private string Area => RouteData.Values["area"] as string;

        public AdminController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<AdminController<TAdminConfigurator>>>();
            _passwordValidator = serviceProvider.GetService<IPasswordValidator>();
            _mapper = serviceProvider.GetService<IMapper>();
            _userByEmailValidator = serviceProvider.GetService<IUserByEmailValidator>();
            //_adminRepository = new AdminRepository<TAdminConfigurator>(serviceProvider);
        }

        [Route("modules/[area]/admin/{model:required}")]
        public IActionResult Admin(string model)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var adminConfig = coreAdminService.GetAdminConfig(model); //_adminRepository.GetAdminConfig(model);
                if (adminConfig != null)
                {
                    ViewBag.AdminConfig = adminConfig;

                }
                ViewBag.Model = model;
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting loading admin: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
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
        [Route("modules/[area]/api/{model:required}")]
        public async Task<IActionResult> GetAllRecords(string model, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
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

        [HttpPost]
        [Route("modules/[area]/api/{model:required}/filter")]
        public async Task<IActionResult> FilterRecords(string model, int pageNo, [FromBody] FilterNode filter, int pageSize = Globals.AdminDefaultPageCount, string orderBy = null)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.GetAllFor(modelType, pageNo, pageSize, orderBy, filter); //_adminRepository.GetAllFor(model, pageNo, pageSize, orderBy);
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
        [Route("modules/[area]/api/{model:required}/tree")]
        public async Task<IActionResult> GetTree(string model)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.GetTree(modelType);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting tree for model: {model}", ex);
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
        public async Task<IActionResult> Create(string model, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.CreateItemFor(modelType, modelObject); //_adminRepository.CreateItemFor(model, fieldObject);
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
        public async Task<IActionResult> Update(string model, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.UpdateItemFor(modelType, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
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
        [Route("modules/[area]/api/{model:required}/tree")]
        public async Task<IActionResult> UpdateTree(string model, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.UpdateTreeFor(modelType, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while updating tree for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/sort/{childModel?}")]
        public async Task<IActionResult> SortItems(string model, string childModel, [FromBody] object modelObject, int pageNo = 1, int pageSize = Globals.AdminDefaultPageCount)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);

                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.SortItemsFor(modelType, pageNo, pageSize, modelObject, childModel); //_adminRepository.UpdateItemFor(model, fieldObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while sorting items for model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/grid/{actionName:required}")]
        public async Task<IActionResult> ExecuteGridAction(string model, string actionName, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.ExecuteGridAction(modelType, actionName, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
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
        [Route("modules/[area]/api/{model:required}/mainform/{actionName:required}")]
        public async Task<IActionResult> ExecuteMainFormAction(string model, string actionName, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.ExecuteMainFormAction(modelType, actionName, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
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
        public async Task<IActionResult> CustomFormSubmit(string model, string formName, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetCustomFormModelType(model, formName);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.CustomFormSubmit(model, formName, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
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
        public async Task<IActionResult> ExecuteCustomFormAction(string model, string formName, string actionName, [FromBody] object modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                var result = await coreAdminService.ExecuteCustomFormAction(modelType, formName, actionName, modelObject); //_adminRepository.UpdateItemFor(model, fieldObject);
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

        [HttpPut]
        [Route("deviser/admin/validator/password")]
        public async Task<IActionResult> ValidatePassword([FromBody] User userDto)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDto);
                var result = _passwordValidator.Validate(user, userDto.Password);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating password, email: {userDto.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("deviser/admin/validator/emailexist")]
        public async Task<IActionResult> ValidateEmail([FromBody] User userDto)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDto);
                var result = _userByEmailValidator.Validate(user);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating user, email: {userDto.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("deviser/admin/validator/userexist")]
        public async Task<IActionResult> ValidateUser([FromBody] User userDto)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDto);
                var result = _userByEmailValidator.Validate(user);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating user, email: {userDto.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/validate/{formType:required}/fieldName/{fieldName:required}")]
        [Route("modules/[area]/api/{model:required}/validate/{formType:required}/form/{formName:required}/fieldName/{fieldName:required}")]
        public async Task<IActionResult> CustomValidate(string model, string formType, string formName, string fieldName, [FromBody] object fieldObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                if (string.IsNullOrEmpty(formType))
                {
                    return BadRequest($"formType is required");
                }

                if (formType == "MainForm")
                {
                    var result = await coreAdminService.ExecuteMainFormCustomValidation(modelType, fieldName, fieldObject);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                if (string.IsNullOrEmpty(formName))
                {
                    return BadRequest($"formName is required for ChildForm and CustomForm");
                }


                if (formType == "ChildForm")
                {
                    var result = await coreAdminService.ExecuteChildFormCustomValidation(modelType, formName, fieldName, fieldObject);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                else //if (formType == "CustomForm")
                {
                    var result = await coreAdminService.ExecuteCustomFormCustomValidation(modelType, formName, fieldName, fieldObject);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while executing custom validation for fieldName: {fieldName} in model: {model}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/lookup/{formType:required}/fieldName/{fieldName:required}")]
        public async Task<IActionResult> GetLookupFor(string model, string formType, string formName, string fieldName,
            [FromBody] object filterParam)
        {
            ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
            var modelType = coreAdminService.GetModelType(model);
            if (modelType == null)
            {
                return BadRequest($"Model {model} is not found");
            }

            if (string.IsNullOrEmpty(formType))
            {
                return BadRequest($"formType is required");
            }

            if (formType == "MainForm")
            {
                var result = await coreAdminService.GetLookUpForMainForm(modelType, fieldName, filterParam);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            if (string.IsNullOrEmpty(formName))
            {
                return BadRequest($"formName is required for ChildForm and CustomForm");
            }

            if (formType == "ChildForm")
            {
                var result = await coreAdminService.GetLookUpForChildForm(modelType, formName, fieldName, filterParam);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            else //if (formType == "CustomForm")
            {
                var result = await coreAdminService.GetLookUpForCustomForm(modelType, formName, fieldName, filterParam);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return NotFound();
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/autofill/{formType:required}/fieldName/{fieldName:required}")]
        [Route("modules/[area]/api/{model:required}/autofill/{formType:required}/form/{formName:required}/fieldName/{fieldName:required}")]
        public async Task<IActionResult> AutoFill(string model, string formType, string formName, string fieldName, [FromBody] dynamic modelObject)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                object result = null;
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                if (string.IsNullOrEmpty(formType))
                {
                    return BadRequest($"formType is required");
                }

                if (formType == "MainForm")
                {
                    result = await coreAdminService.AutoFillMainForm(modelType, fieldName, modelObject.fieldValue);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                if (string.IsNullOrEmpty(formName))
                {
                    return BadRequest($"formName is required for ChildForm and CustomForm");
                }


                if (formType == "ChildForm")
                {
                    result = await coreAdminService.AutoFillChildForm(modelType, formName, fieldName, modelObject.fieldValue);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                else //if (formType == "CustomForm")
                {
                    result = await coreAdminService.AutoFillCustomForm(modelType, formName, fieldName, modelObject.fieldValue);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                result = await coreAdminService.AutoFillMainForm(modelType, fieldName, modelObject.fieldValue); //_adminRepository.UpdateItemFor(model, fieldObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while auto-filling for model: {model}, fieldName: {fieldName}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("modules/[area]/api/{model:required}/calculate/{formType:required}/fieldName/{fieldName:required}")]
        [Route("modules/[area]/api/{model:required}/calculate/{formType:required}/form/{formName:required}/fieldName/{fieldName:required}")]
        public async Task<IActionResult> Calculate(string model, string formType, string formName, string fieldName, [FromBody] dynamic basedOnFields)
        {
            try
            {
                ICoreAdminService coreAdminService = new CoreAdminService(Area, _serviceProvider);
                var modelType = coreAdminService.GetModelType(model);
                object result = null;
                if (modelType == null)
                {
                    return BadRequest($"Model {model} is not found");
                }

                if (string.IsNullOrEmpty(formType))
                {
                    return BadRequest($"formType is required");
                }

                if (formType == "MainForm")
                {
                    result = await coreAdminService.CalculateMainForm(modelType, fieldName, basedOnFields);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                if (string.IsNullOrEmpty(formName))
                {
                    return BadRequest($"formName is required for ChildForm and CustomForm");
                }


                if (formType == "ChildForm")
                {
                    result = await coreAdminService.CalculateChildForm(modelType, formName, fieldName, basedOnFields);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                else //if (formType == "CustomForm")
                {
                    result = await coreAdminService.CalculateCustomForm(modelType, formName, fieldName, basedOnFields);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                result = await coreAdminService.CalculateMainForm(modelType, fieldName, basedOnFields); //_adminRepository.UpdateItemFor(model, fieldObject);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while calculating for model: {model}, fieldName: {fieldName}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
