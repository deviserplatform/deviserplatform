using Autofac;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;

using DeviserWI.Controllers.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Library;

namespace Deviser.WI.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //Logger
        private readonly ILogger<UserController> logger;
        private readonly UserManager<Core.Data.Entities.User> userManager;
        private IUserProvider userProvider;
        private IRoleProvider roleProvider;

        public UserController(ILifetimeScope container, UserManager<Core.Data.Entities.User> userManager)
        {
            logger = container.Resolve<ILogger<UserController>>();
            userProvider = container.Resolve<IUserProvider>();
            roleProvider = container.Resolve<IRoleProvider>();
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = userProvider.GetUsers();
                if (users != null)
                {
                    List<Core.Library.DomainTypes.User> result = Mapper.Map<List<Core.Library.DomainTypes.User>>(users);
                    //Roles workarround
                    foreach(var user in users)
                    {
                        if(user.Roles!=null && user.Roles.Count > 0)
                        {
                            var targetUser = result.First(u => u.Id == user.Id);
                            targetUser.Roles = new List<Role>();
                            foreach (var userRole in user.Roles)
                            {
                                if (userRole != null)
                                {
                                    var role = roleProvider.GetRole(userRole.RoleId);
                                    targetUser.Roles.Add(role);
                                }
                            }
                            
                        }
                    }
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting skins"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Core.Library.DomainTypes.User userDTO)
        {
            try
            {
                if(userDTO!=null)
                {
                    var user = new Core.Data.Entities.User();
                    Mapper.Map(userDTO, user, typeof(Core.Library.DomainTypes.User), typeof(Core.Data.Entities.User));
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = userDTO.Email;
                    var result = await userManager.CreateAsync(user, userDTO.password);
                    if (result.Succeeded)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest();
                }

                
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while creating new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Core.Library.DomainTypes.User userDTO)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(u => u.Id == userDTO.Id);
                Mapper.Map(userDTO, user, typeof(Core.Library.DomainTypes.User), typeof(Core.Data.Entities.User));
                var result = userManager.UpdateAsync(user).Result;
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {                
                logger.LogError(string.Format("Error occured while updating user Id: {0}, {1}", userDTO.Id, ex));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while deleting user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("isexist")]
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
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while checking user exist"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("passwordreset")]
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
                        var result = userManager.ChangePasswordAsync(user, currentPassword, newPassword).Result;
                        return Ok(result);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured  while resetting password"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("role")]
        [HttpPost]
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
                        var resultUser = userProvider.GetUser(userId); //userManager.Users.FirstOrDefault(u => u.Id == userId);
                        var userDTO = ConvertToUserDTO(resultUser); 
                        return Ok(userDTO);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
                
        [HttpDelete("role/{userId}/{roleName}")]
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
                        var resultUser = userProvider.GetUser(userId);
                        var userDTO = ConvertToUserDTO(resultUser);
                        return Ok(userDTO);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private Core.Library.DomainTypes.User ConvertToUserDTO(Core.Data.Entities.User user)
        {
            var userDTO = Mapper.Map<Core.Library.DomainTypes.User>(user);
            if (user.Roles != null && user.Roles.Count > 0)
            {
                userDTO.Roles = new List<Role>();
                foreach (var userRole in user.Roles)
                {
                    if (userRole != null)
                    {
                        var role = roleProvider.GetRole(userRole.RoleId);
                        userDTO.Roles.Add(role);
                    }

                }
            }
            return userDTO;
        }
    }
}
