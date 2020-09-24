using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Deviser.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators")]
    public class UserController : Controller
    {
        //Logger
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<Core.Data.Entities.User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserController(ILogger<UserController> logger, 
            IMapper mapper,
            UserManager<Core.Data.Entities.User> userManager,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = _userRepository.GetUsers();
                if (users != null)
                {
                    var result = users;                    
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting themes"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User userDTO)
        {
            try
            {
                if(userDTO!=null)
                {   
                    var user = _mapper.Map<Core.Data.Entities.User>(userDTO);
                    user.Id = Guid.NewGuid();
                    user.UserName = userDTO.Email;
                    var result = await _userManager.CreateAsync(user, userDTO.Password);
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
                _logger.LogError(string.Format("Error occured  while creating new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] User userDTO)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDTO);
                var result = _userManager.UpdateAsync(user).Result;
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {                
                _logger.LogError(string.Format("Error occured while updating user Id: {0}, {1}", userDTO.Id, ex));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var result = _userManager.DeleteAsync(user).Result;
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while deleting user"), ex);
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
                    var result = _userManager.Users.Any(u => u.UserName == userName);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured  while checking user exist"), ex);
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
                    var userId = (Guid)passwordObj.userId;
                    var currentPassword = passwordObj.currentPassword;
                    var newPassword = passwordObj.newPassword;
                    if (userId!= Guid.Empty && !string.IsNullOrEmpty(newPassword))
                    {
                        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                        var result = _userManager.ChangePasswordAsync(user, currentPassword, newPassword).Result;
                        return Ok(result);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured  while resetting password"), ex);
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
                    var userId = (Guid)userRoleObj.userId;
                    var roleName = (string)userRoleObj.roleName;
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                    var result = _userManager.AddToRoleAsync(user, roleName).Result;
                    if (result.Succeeded)
                    {
                        var resultUser = _userRepository.GetUser(userId); //userManager.Users.FirstOrDefault(u => u.Id == userId);
                        return Ok(resultUser);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
                
        [HttpDelete("role/{userId}/{roleName}")]
        public IActionResult RemoveRole(Guid userId, string roleName)
        {
            try
            {
                if (userId != null && roleName != null)
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                    var result = _userManager.RemoveFromRoleAsync(user, roleName).Result;
                    if (result.Succeeded)
                    {
                        var resultUser = _userRepository.GetUser(userId);
                        return Ok(resultUser);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a new user"), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
