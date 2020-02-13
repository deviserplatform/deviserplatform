using Deviser.Admin.Config;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Admin.Extensions
{
    public static class ValidationExtensions
    {
        public static IEnumerable<ValidationError> ToValidationError(this IEnumerable<IdentityError> identityErrors)
        {
            return identityErrors.Select(ie => new ValidationError()
            {
                Code = ie.Code,
                Description = ie.Description
            }).ToList();
        }
    }
}
