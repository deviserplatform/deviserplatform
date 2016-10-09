using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {

        //Logger
        private readonly ILogger<RoleController> logger;

        IRoleProvider roleProvider;

        public RoleController(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<RoleController>>();
            roleProvider = container.Resolve<IRoleProvider>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var dbResult = roleProvider.GetRoles();
                var result = Mapper.Map<List<Deviser.Core.Common.DomainTypes.Role>>(dbResult);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all roles"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = roleProvider.GetRole(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting a role, roleId: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Deviser.Core.Common.DomainTypes.Role role)
        {
            try
            {
                var result = roleProvider.CreateRole(role);
                if (result != null)
                    return Ok(result);
                return BadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a role, roleObj: ", JsonConvert.SerializeObject(role)), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Deviser.Core.Common.DomainTypes.Role role)
        {
            try
            {
                if (role != null)
                {
                    var result = roleProvider.UpdateRole(role);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating role");
                logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id!= Guid.Empty)
                {
                    var result = roleProvider.DeleteRole(id);
                    if (result != null)
                        return Ok();
                }
                return BadRequest("Invalid role id");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting role, roleId: ", id);
                logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
