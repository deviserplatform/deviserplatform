using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.Sites
{
    public interface ISettingManager
    {
        SMTPSetting GetSMTPSetting();
        SiteSetting GetSiteSetting();
    }
}