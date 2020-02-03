using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{

    public interface ISiteSettingRepository
    {
        List<SiteSetting> GetSettings();
        string GetSettingValue(string settingName);
        List<SiteSetting> UpdateSetting(List<SiteSetting> settings);
    }

    public class SiteSettingRepository : AbstractRepository, ISiteSettingRepository
    {
        //Logger
        private readonly ILogger<SiteSettingRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public SiteSettingRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<SiteSettingRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        //Custom Field Declaration
        public List<SiteSetting> GetSettings()
        {
            try
            {
                //var cacheName = nameof(GetSettings);
                //var result = GetResultFromCache<List<SiteSetting>>(cacheName);
                //if (result != null)
                //{
                //    return result;
                //}

                using (var context = new DeviserDbContext(_dbOptions))
                {
                    var dbResult = context.SiteSetting.ToList();                    
                    var result = _mapper.Map<List<SiteSetting>>(dbResult);
                    //AddResultToCache(cacheName, result);
                    return result;
                }
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
                using (var context = new DeviserDbContext(_dbOptions))
                {
                    var setting = context.SiteSetting.FirstOrDefault(s=>s.SettingName==settingName);
                    if (setting != null)
                        return setting.SettingValue;
                }
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
                using (var context = new DeviserDbContext(_dbOptions))
                {
                    var dbSettings = _mapper.Map<List<Entities.SiteSetting>>(settings);
                    context.SiteSetting.UpdateRange(dbSettings);
                    context.SaveChanges();
                    var result = context.SiteSetting.ToList();
                    if (result != null)
                        return _mapper.Map<List<SiteSetting>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating settings", ex);
            }
            return null;
        }

    }

}//End namespace
