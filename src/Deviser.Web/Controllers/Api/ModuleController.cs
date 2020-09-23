using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Deviser.Core.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Module = Deviser.Core.Common.DomainTypes.Module;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    [PermissionAuthorize("PAGE", "EDIT")]
    public class ModuleController : Controller
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(ILogger<ModuleController> logger,
        IModuleRepository moduleRepository)
        {
            _logger = logger;
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var result = _moduleRepository.GetModules();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all modules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _moduleRepository.GetModule(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting Module: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("moduleviewtype/")]
        [AllowAnonymous]
        public IActionResult GetModuleViewType()
        {
            try
            {   
                var result = _moduleRepository.GetModuleViewType();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting module action"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Module module)
        {
            try
            {
                var result = _moduleRepository.Create(module);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]Module module)
        {
            try
            {
                var result = _moduleRepository.UpdateModule(module);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var module = _moduleRepository.GetModule(id);
                if (module != null)
                {
                    module.IsActive = true; //is it correct?
                    var result = _moduleRepository.UpdateModule(module);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
