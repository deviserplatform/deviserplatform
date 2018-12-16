using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Data.Entities;

namespace Deviser.Admin.Validation
{
    public interface IUserByEmailValidator
    {
        ValidationResult Validate(User user);
    }
}