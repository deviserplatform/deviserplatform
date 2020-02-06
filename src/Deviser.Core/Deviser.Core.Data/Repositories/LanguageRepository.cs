using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public interface ILanguageRepository
    {
        Language CreateLanguage(Language dbLanguage);
        List<Language> GetLanguages();
        List<Language> GetActiveLanguages();
        Language GetLanguage(Guid languageId);
        bool IsMultilingual();
        Language UpdateLanguage(Language dbLanguage);
    }

    public class LanguageRepository : ILanguageRepository
    {
        //Logger
        private readonly ILogger<LanguageRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public LanguageRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<LanguageRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
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
                return _mapper.Map<Language>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating Language", ex);
            }
            return null;
        }

        //Custom Field Declaration
        public List<Language> GetLanguages()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Language
                    .ToList();

                return _mapper.Map<List<Language>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public List<Language> GetActiveLanguages()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Language
                    .Where(l => l.IsActive)
                    .ToList();

                return _mapper.Map<List<Language>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Language
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
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Language
                             .Count(l => l.IsActive) > 1;

            return result;

            //var result = GetActiveLanguages();
            //return result != null && result.Count > 1;
        }

        public Language UpdateLanguage(Language language)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbLanguage = _mapper.Map<Entities.Language>(language);
                var result = context.Language.Update(dbLanguage).Entity;
                //var result = context.Language.Attach(dbLanguage).Entity;
                //context.Entry(dbLanguage).State = EntityState.Modified;
                context.SaveChanges();
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
