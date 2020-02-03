using Deviser.Admin.Config;
using Deviser.Core.Data.Entities;

namespace Deviser.Admin.Validation
{
    public interface IPasswordValidator
    {
        ValidationResult Validate(User user, string password);
    }
}