using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin.Extensions;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Admin.Validation
{
    public class PasswordValidator : IPasswordValidator
    {
        private readonly UserManager<User> _userManager;
        public PasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public ValidationResult Validate(User user, string password)
        {
            var errors = new List<IdentityError>();
            foreach (var v in _userManager.PasswordValidators)
            {
                var result = v.ValidateAsync(_userManager, user, password).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {                
                return ValidationResult.Failed(errors.ToValidationError().ToArray());
            }
            return ValidationResult.Success;
        }
    }
}
