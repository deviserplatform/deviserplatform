using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILanguageProvider
    {
        Language CreateLanguage(Language dbLanguage);
        List<Language> GetLanguages();
        List<Language> GetActiveLanguages();
        Language GetLanguage(Guid languageId);
        bool IsMultilingual();
        Language UpdateLanguage(Language dbLanguage);
    }

    public class LanguageProvider : DataProviderBase, ILanguageProvider
    {
        //Logger
        private readonly ILogger<LanguageProvider> _logger;

        //Constructor
        public LanguageProvider(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<LanguageProvider>>();
        }

        public Language CreateLanguage(Language language)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                var dbLanguage = Mapper.Map<Entities.Language>(language);
                dbLanguage.CreatedDate = dbLanguage.LastModifiedDate = DateTime.Now;
                dbLanguage.IsActive = true;

                var result = context.Language.Add(dbLanguage).Entity;
                context.SaveChanges();
                return Mapper.Map<Language>(result);
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                var result = context.Language
                            .ToList();

                return Mapper.Map<List<Language>>(result);
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                var result = context.Language
                    .Where(l => l.IsActive)
                            .ToList();

                return Mapper.Map<List<Language>>(result);
                //}
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
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                var result = context.Language
                    .FirstOrDefault(e => e.Id == languageId);

                return Mapper.Map<Language>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public bool IsMultilingual()
        {
            using (var context = new DeviserDbContext(DbOptions))
            {

                var result = context.Language
                    .Where(l => l.IsActive)
                    .Count() > 1;

                return result;
            }

            //var result = GetActiveLanguages();
            //return result != null && result.Count > 1;
        }

        public Language UpdateLanguage(Language language)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                var dbLanguage = Mapper.Map<Entities.Language>(language);
                var result = context.Language.Attach(dbLanguage).Entity;
                context.Entry(dbLanguage).State = EntityState.Modified;
                context.SaveChanges();
                return Mapper.Map<Language>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating Language", ex);
            }
            return null;
        }
    }
}
