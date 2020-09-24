using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface ISiteSettingRepository
    {
        List<SiteSetting> GetSettings();
        IDictionary<string, string> GetSettingsAsDictionary();
        string GetSettingValue(string settingName);
        List<SiteSetting> UpdateSetting(List<SiteSetting> settings);
    }
}