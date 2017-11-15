using Autofac;
using Deviser.Core.Data.DataProviders;
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
        private ILogger<LayoutController> logger;

        IModuleProvider moduleProvider;

        public ModuleController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LayoutController>>();
            moduleProvider = container.Resolve<IModuleProvider>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = moduleProvider.Get();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all modules"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = moduleProvider.Get(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting Module: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("moduleactiontype/")]
        public IActionResult GetModuleActionType()
        {
            try
            {
                var moduleActionTypes = moduleProvider.GetModuleActionType();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.ModuleActionType>>(moduleActionTypes);

                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting content types"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Module module)
        {
            try
            {
                var result = moduleProvider.Create(module);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a Module"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]Module module)
        {
            try
            {
                var result = moduleProvider.Update(module);
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
                var module = moduleProvider.Get(id);
                if (module != null)
                {
                    module.IsActive = true; //is it correct?
                    var result = moduleProvider.Update(module);
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
