using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;

namespace Deviser.Core.Data.DataProviders
{

    public interface ISiteSettingProvider
    {
        List<SiteSetting> GetSettings();
        string GetSettingValue(string settingName);
        List<SiteSetting> UpdateSetting(List<SiteSetting> settings);
    }

    public class SiteSettingProvider : DataProviderBase, ISiteSettingProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public SiteSettingProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<SiteSetting> returnData = context.SiteSetting.ToList();
                    return new List<SiteSetting>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

        public string GetSettingValue(string settingName)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var setting = context.SiteSetting.FirstOrDefault(s=>s.SettingName==settingName);
                    if (setting != null)
                        return setting.SettingValue;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

        public List<SiteSetting> UpdateSetting(List<SiteSetting> settings)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    context.SiteSetting.UpdateRange(settings);
                    context.SaveChanges();
                    var result = context.SiteSetting.ToList();
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating settings", ex);
            }
            return null;
        }

    }

}//End namespace
