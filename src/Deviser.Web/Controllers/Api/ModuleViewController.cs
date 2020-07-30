using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;


namespace Deviser.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/ModuleView")]
    public class ModuleViewController : Controller
    {
        private readonly ILogger<ModuleViewController> _logger;
        private readonly IModuleRepository _moduleRepository;

        public ModuleViewController(ILogger<ModuleViewController> logger,
            IModuleRepository moduleRepository)
        {
            _logger = logger;
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _moduleRepository.GetModuleViews();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting modules views"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("edit/{moduleId}")]
        public IActionResult GetEditViews(Guid moduleId)
        {
            try
            {
                var result = _moduleRepository.GetEditModuleViews(moduleId);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting edit modules views"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ModuleView moduleView)
        {
            try
            {
                var result = _moduleRepository.CreateModuleView(moduleView); 
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ModuleView moduleViews)
        {
            try
            {
                var result = _moduleRepository.UpdateModuleView(moduleViews);
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
                var moduleView = _moduleRepository.GetModule(id); 
                if (moduleView != null)
                {
                    moduleView.IsActive = false; //is it correct?
                    var result = _moduleRepository.UpdateModule(moduleView);
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