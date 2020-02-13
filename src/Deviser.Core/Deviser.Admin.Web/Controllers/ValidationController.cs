using AutoMapper;
using Deviser.Admin.Validation;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Deviser.Admin.Web.Controllers
{
    public class ValidationController : Controller
    {
        //Logger
        private readonly ILogger<ValidationController> _logger;
        private readonly IMapper _mapper;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUserByEmailValidator _userByEmailValidator;


        public ValidationController(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<ValidationController>>();
            _passwordValidator = serviceProvider.GetService<IPasswordValidator>();
            _mapper = serviceProvider.GetService<IMapper>();
            _userByEmailValidator = serviceProvider.GetService<IUserByEmailValidator>();
        }

        [HttpPost]
        [Route("deviser/admin/validator/password")]
        public async Task<IActionResult> ValidatePassword([FromBody] User userDTO)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDTO);
                var result = _passwordValidator.Validate(user, userDTO.Password);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating password, email: {userDTO.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("deviser/admin/validator/emailexist")]
        public async Task<IActionResult> ValidateEmail([FromBody] User userDTO)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDTO);
                var result = _userByEmailValidator.Validate(user);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating user, email: {userDTO.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("deviser/admin/validator/userexist")]
        public async Task<IActionResult> ValidateUser([FromBody] User userDTO)
        {
            try
            {
                var user = _mapper.Map<Core.Data.Entities.User>(userDTO);
                var result = _userByEmailValidator.Validate(user);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while validating user, email: {userDTO.Email}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
