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
    public interface ILanguageRepository
    {
        Language CreateLanguage(Language dbLanguage);
        List<Language> GetLanguages(bool refreshCache = false);
        List<string> GetActiveLocales();
        List<Language> GetActiveLanguages();
        Language GetLanguage(Guid languageId);
        bool IsMultilingual();
        Language UpdateLanguage(Language dbLanguage);
    }

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
            try
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
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating Language", ex);
            }
            return null;
        }

        //Custom Field Declaration
        public List<Language> GetLanguages(bool refreshCache = false)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public List<string> GetActiveLocales()
        {
            try
            {
                var activeLanguages = GetActiveLanguages();
                var result = activeLanguages
                    .Select(l => l.CultureCode)
                    .ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting active languages", ex);
            }
            return null;
        }

        public List<Language> GetActiveLanguages()
        {
            try
            {
                var languages = GetLanguages();
                var activeLanguages= languages
                    .Where(l => l.IsActive)
                    .ToList();
                return activeLanguages;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting active languages", ex);
            }
            return null;
        }

        public Language GetLanguage(Guid languageId)
        {
            try
            {
                var languages = GetLanguages();
                var result = languages
                    .FirstOrDefault(e => e.Id == languageId);

                return _mapper.Map<Language>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public bool IsMultilingual()
        {
            var result = GetActiveLanguages();
            return result != null && result.Count > 1;
        }

        public Language UpdateLanguage(Language language)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbLanguage = context.Language.ToList().FirstOrDefault(l => string.Equals(language.CultureCode, l.CultureCode, StringComparison.InvariantCultureIgnoreCase));
                if (dbLanguage == null)
                {
                    return null;
                }

                _mapper.Map(language, dbLanguage);
                var result = context.Language.Update(dbLanguage).Entity;
                context.SaveChanges();
                //Refresh Language Cache
                GetLanguages(true);
                return _mapper.Map<Language>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating Language", ex);
            }
            return null;
        }
    }
}
