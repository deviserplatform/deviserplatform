using Deviser.Admin.Config;
using Deviser.Admin.Extensions;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Deviser.Admin.Validation
{
    public class UserValidator: IUserByEmailValidator
    {
        private readonly UserManager<User> _userManager;
        public UserValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public ValidationResult Validate(User user)
        {
            //if (_userManager.SupportsUserSecurityStamp)
            //{
            //    var stamp = _userManager.GetSecurityStampAsync(user).GetAwaiter().GetResult();
            //    if (stamp == null)
            //    {
            //        throw new InvalidOperationException("NullSecurityStamp");
            //    }
            //}

           if (string.IsNullOrEmpty(user.UserName))
            {
                //Assuming the platform users email as user name
                user.UserName = user.Email;
            }
            
            var errors = new List<ValidationError>();
            foreach (var v in _userManager.UserValidators)
            {
                var result = v.ValidateAsync(_userManager, user).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors.ToValidationError());
                }
            }
            if (errors.Count > 0)
            {
                //Logger.LogWarning(13, "User {userId} validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return ValidationResult.Failed(errors.ToArray());
            }
            return ValidationResult.Success;
        }
    }
}
