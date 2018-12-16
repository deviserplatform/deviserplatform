using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Admin.Validation
{
    public interface IPasswordValidator
    {
        ValidationResult Validate(User user, string password);
    }
}