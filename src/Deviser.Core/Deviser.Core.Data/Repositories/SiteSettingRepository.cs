using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public class SiteSettingRepository : AbstractRepository, ISiteSettingRepository
    {
        private readonly ILogger<SiteSettingRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        public SiteSettingRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<SiteSettingRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public List<SiteSetting> GetSettings()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbResult = context.SiteSetting.ToList();
            var result = _mapper.Map<List<SiteSetting>>(dbResult);
            return result;
        }

        public IDictionary<string, string> GetSettingsAsDictionary()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.SiteSetting.ToDictionary(s => s.SettingName, v => v.SettingValue);
            return result;
        }

        public string GetSettingValue(string settingName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var setting = context.SiteSetting.FirstOrDefault(s => s.SettingName == settingName);
            if (setting == null) throw new InvalidOperationException($"Unable to find setting name {settingName}");
            return setting.SettingValue;
        }

        public List<SiteSetting> UpdateSetting(List<SiteSetting> settings)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbSettings = _mapper.Map<List<Entities.SiteSetting>>(settings);
            context.SiteSetting.UpdateRange(dbSettings);
            context.SaveChanges();
            var result = context.SiteSetting.ToList();
            return _mapper.Map<List<SiteSetting>>(result);
        }

    }
}
