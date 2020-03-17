using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.Sites
{
    public interface ISettingManager
    {
        SMTPSetting GetSMTPSetting();
        SiteSettingInfo GetSiteSetting();
        string GetSettingValue(string settingName);
        SiteSettingInfo UpdateSettingInfo(SiteSettingInfo settingInfo);
    }
}