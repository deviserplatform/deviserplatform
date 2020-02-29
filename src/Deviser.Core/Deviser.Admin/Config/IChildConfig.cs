using Deviser.Admin.Config;

namespace Deviser.Admin
{
    public interface IChildConfig : IAdminBaseConfig
    {
        Field Field { get; }
    }
}