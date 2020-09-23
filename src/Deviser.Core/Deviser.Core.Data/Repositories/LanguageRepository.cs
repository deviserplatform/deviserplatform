using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Cache;

namespace Deviser.Core.Data.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        //Logger
        private readonly ILogger<LanguageRepository> _logger;
        private readonly IDeviserDataCache _deviserDataCache;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public LanguageRepository(DbContextOptions<DeviserDbContext> dbOptions,
            IDeviserDataCache deviserDataCache,
            ILogger<LanguageRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _deviserDataCache = deviserDataCache;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public Language CreateLanguage(Language language)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbLanguage = _mapper.Map<Entities.Language>(language);
            dbLanguage.CreatedDate = dbLanguage.LastModifiedDate = DateTime.Now;
            dbLanguage.IsActive = true;

            var result = context.Language.Add(dbLanguage).Entity;
            context.SaveChanges();
            //Refresh Language Cache
            GetLanguages(true);
            return _mapper.Map<Language>(result);
        }

        //Custom Field Declaration
        public List<Language> GetLanguages(bool refreshCache = false)
        {
            if (_deviserDataCache.ContainsKey(nameof(GetLanguages)) && !refreshCache)
            {
                var cacheResult = _deviserDataCache.GetItem<List<Entities.Language>>(nameof(GetLanguages));
                return _mapper.Map<List<Language>>(cacheResult);
            }

            using var context = new DeviserDbContext(_dbOptions);
            var dbLanguages = context.Language
                .ToList();
            var result = _mapper.Map<List<Language>>(dbLanguages);
            _deviserDataCache.AddOrUpdate(nameof(GetLanguages), dbLanguages);
            return _mapper.Map<List<Language>>(result);

        }

        public List<string> GetActiveLocales()
        {
            var activeLanguages = GetActiveLanguages();
            var result = activeLanguages
                .Select(l => l.CultureCode)
                .ToList();
            return result;
        }

        public List<Language> GetActiveLanguages()
        {
            var languages = GetLanguages();
            var activeLanguages = languages
                .Where(l => l.IsActive)
                .ToList();
            return activeLanguages;
        }

        public Language GetLanguage(Guid languageId)
        {
            var languages = GetLanguages();
            var result = languages
                .FirstOrDefault(e => e.Id == languageId);

            return _mapper.Map<Language>(result);
        }

        public bool IsMultilingual()
        {
            var result = GetActiveLanguages();
            return result != null && result.Count > 1;
        }

        public Language UpdateLanguage(Language language)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbLanguage = context.Language.ToList().FirstOrDefault(l => string.Equals(language.CultureCode, l.CultureCode, StringComparison.InvariantCultureIgnoreCase));
            if (dbLanguage == null)
            {
                return null;
            }

            //_mapper.Map(language, dbLanguage);
            dbLanguage.IsActive = language.IsActive;
            var result = _mapper.Map<Language>(dbLanguage); //context.Language.Update(dbLanguage).Entity;
            context.SaveChanges();
            //Refresh Language Cache
            GetLanguages(true);
            return _mapper.Map<Language>(result);
        }
    }
}
