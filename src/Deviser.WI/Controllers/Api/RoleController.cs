using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace DeviserWI.Controllers.API
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {

        //Logger
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleRepository _roleRepository;

        public RoleController(ILogger<RoleController> logger, 
            IRoleRepository roleRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _roleRepository.GetRoles();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting all roles"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _roleRepository.GetRole(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting a role, roleId: {0}", id), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Deviser.Core.Common.DomainTypes.Role role)
        {
            try
            {
                var result = _roleRepository.CreateRole(role);
                if (result != null)
                    return Ok(result);
                return BadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a role, roleObj: ", JsonConvert.SerializeObject(role)), ex);
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
                    var result = _roleRepository.UpdateRole(role);
                    if (result != null)
                        return Ok(result);
                }
                return BadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating role");
                _logger.LogError(errorMessage, ex);
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
                    var result = _roleRepository.DeleteRole(id);
                    if (result != null)
                        return Ok();
                }
                return BadRequest("Invalid role id");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting role, roleId: ", id);
                _logger.LogError(errorMessage, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
