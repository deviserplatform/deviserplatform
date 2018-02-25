using Autofac;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Module = Deviser.Core.Common.DomainTypes.Module;
using AutoMapper;

namespace DeviserWI.Controllers.API
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ModuleController : Controller
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<ModuleController>>();
            _moduleRepository = container.Resolve<IModuleRepository>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _moduleRepository.Get();
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
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _moduleRepository.Get(id);
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
        [Route("moduleactiontype/")]
        public IActionResult GetModuleActionType()
        {
            try
            {   
                var result = _moduleRepository.GetModuleActionType();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting content types"), ex);
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
                var result = _moduleRepository.Update(module);
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
                var module = _moduleRepository.Get(id);
                if (module != null)
                {
                    module.IsActive = true; //is it correct?
                    var result = _moduleRepository.Update(module);
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
