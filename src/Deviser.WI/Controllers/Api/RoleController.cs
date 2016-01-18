using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.Core.Library.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

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
                var result = roleProvider.GetRoles();
                if (result != null)
                {
                    return Ok(result);
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all roles"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult Get(string roleId)
        {
            try
            {
                var result = roleProvider.GetRole(roleId);
                if (result != null)
                    return Ok(result);
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting a role, roleId: {0}", roleId), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post(Role role)
        {
            try
            {
                var result = roleProvider.CreateRole(role);
                if (result != null)
                    return Ok(result);
                return HttpBadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a role, roleObj: ", JsonConvert.SerializeObject(role)), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put(Role role)
        {
            try
            {
                if (role != null)
                {
                    var result = roleProvider.UpdateRole(role);
                    if (result != null)
                        return Ok(result);
                }
                return HttpBadRequest("Invalid role");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while updating role");
                logger.LogError(errorMessage, ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var result = roleProvider.DeleteRole(id);
                    if (result != null)
                        return Ok();
                }
                return HttpBadRequest("Invalid role id");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error occured while deleting role, roleId: ", id);
                logger.LogError(errorMessage, ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
