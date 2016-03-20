using Autofac;
using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.WI.DTO;
using DeviserWI.Controllers.API;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //Logger
        private readonly ILogger<UserController> logger;
        private readonly UserManager<Core.Data.Entities.User> userManager;

        public UserController(ILifetimeScope container, UserManager<Core.Data.Entities.User> userManager)
        {
            logger = container.Resolve<ILogger<UserController>>();
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = userManager.Users.ToList();
                if (users != null)
                {
                    List<DTO.User> result = Mapper.Map<List<DTO.User>>(users);
                    return Ok(result);
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting skins"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult PasswordReset([FromBody] DTO.User userDTO)
        {
            try
            {
                var user = new Core.Data.Entities.User();
                Mapper.Map(userDTO, user, typeof(DTO.User), typeof(Core.Data.Entities.User));
                user.UserName = userDTO.Email;
                var result = userManager.CreateAsync(user, userDTO.password).Result;
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while creating new user"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] DTO.User userDTO)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(u => u.Id == userDTO.Id);
                Mapper.Map(userDTO, user, typeof(DTO.User), typeof(Core.Data.Entities.User));
                var result = userManager.UpdateAsync(user).Result;
                if (result != null)
                    return Ok(result);
                return HttpBadRequest();
            }
            catch (Exception ex)
            {                
                logger.LogError(string.Format("Error occured while updating user Id: {0}, {1}", userDTO.Id, ex));
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var result = userManager.DeleteAsync(user).Result;
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting user"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("/isexist")]
        public IActionResult IsUserExist([FromBody]dynamic userObj)
        {
            try
            {
                string userName = userObj.userName;
                if (!string.IsNullOrEmpty(userName))
                {
                    var result = userManager.Users.Any(u => u.UserName == userName);
                    return Ok(result);
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while checking user exist"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("/passwordreset")]
        public IActionResult PasswordReset([FromBody]dynamic passwordObj)
        {
            try
            {
                if (passwordObj != null && passwordObj.userId != null &&
                    passwordObj.currentPassword != null && passwordObj.newPassword != null)
                {
                    string userId = passwordObj.userId;
                    string currentPassword = passwordObj.currentPassword;
                    string newPassword = passwordObj.newPassword;
                    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userId) &&
                        !string.IsNullOrEmpty(newPassword))
                    {
                        var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
                        var result = userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                        return Ok(result);
                    }
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while resetting password"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
                
        [HttpPost("/role")]
        public IActionResult AddRole([FromBody] dynamic userRoleObj)
        {
            try
            {
                if (userRoleObj != null && userRoleObj.userId != null && userRoleObj.roleName != null)
                {
                    string userId = (string)userRoleObj.userId;
                    string roleName = (string)userRoleObj.roleName;
                    var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
                    var result = userManager.AddToRoleAsync(user, roleName).Result;
                    if (result.Succeeded)
                    {
                        var resultUser = userManager.Users.FirstOrDefault(u => u.Id == userId);
                        var userDTO = Mapper.Map<DTO.User>(resultUser);
                        return Ok(userDTO);
                    }
                }
                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
                
        [HttpDelete("/role/{userId}/{roleName}")]
        public IActionResult RemoveRole(string userId, string roleName)
        {
            try
            {
                if (userId != null && roleName != null)
                {
                    var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
                    var result = userManager.RemoveFromRoleAsync(user, roleName).Result;
                    if (result.Succeeded)
                    {
                        var resultUser = userManager.Users.FirstOrDefault(u => u.Id == userId);
                        var userDTO = Mapper.Map<DTO.User>(resultUser);
                        return Ok(userDTO);
                    }
                }

                return HttpBadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
