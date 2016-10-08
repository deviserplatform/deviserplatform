using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;


namespace Deviser.WI.Controllers
{
    [Produces("application/json")]
    [Route("api/ModuleAction")]
    public class ModuleActionController : Controller
    {
        private ILogger<ModuleActionController> logger;

        IModuleProvider moduleProvider;

        public ModuleActionController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<ModuleActionController>>();
            moduleProvider = container.Resolve<IModuleProvider>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = moduleProvider.GetModuleActions();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting modules actions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("edit/{moduleId}")]
        public IActionResult GetEditActions(Guid moduleId)
        {
            try
            {
                var result = moduleProvider.GetEditModuleActions(moduleId);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting edit modules actions"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ModuleAction moduleAction)
        {
            try
            {
                var result = moduleProvider.CreateModuleAction(moduleAction); 
                if (result != null)
                    return Ok(result);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ModuleAction moduleActions)
        {
            try
            {
                var result = moduleProvider.UpdateModuleAction(moduleActions);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var moduleActions = moduleProvider.Get(id); 
                if (moduleActions != null)
                {
                    moduleActions.IsActive = false; //is it correct?
                    var result = moduleProvider.Update(moduleActions);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}