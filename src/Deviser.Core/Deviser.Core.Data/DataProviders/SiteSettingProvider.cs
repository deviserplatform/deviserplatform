using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;

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
        private readonly ILogger<LayoutProvider> _logger;

        //Constructor
        public SiteSettingProvider(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.SiteSetting.ToList();
                    return Mapper.Map<List<SiteSetting>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

        public string GetSettingValue(string settingName)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var setting = context.SiteSetting.FirstOrDefault(s=>s.SettingName==settingName);
                    if (setting != null)
                        return setting.SettingValue;
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting GetSettings", ex);
            }
            return null;
        }

        public List<SiteSetting> UpdateSetting(List<SiteSetting> settings)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbSettings = Mapper.Map<List<Entities.SiteSetting>>(settings);
                    context.SiteSetting.UpdateRange(dbSettings);
                    context.SaveChanges();
                    var result = context.SiteSetting.ToList();
                    if (result != null)
                        return Mapper.Map<List<SiteSetting>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating settings", ex);
            }
            return null;
        }

    }

}//End namespace
