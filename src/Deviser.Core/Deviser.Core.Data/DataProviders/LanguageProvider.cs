using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILanguageProvider
    {
        List<Language> GetLanguages();
        List<Language> GetActiveLanguages();
        Language GetLanguage(Guid languageId);
        Language CreateLanguage(Language language);
        Language UpdateLanguage(Language language);
        bool IsMultilingual();

    }

    public class LanguageProvider : DataProviderBase, ILanguageProvider
    {
        //Logger
        private readonly ILogger<LanguageProvider> logger;

        //Constructor
        public LanguageProvider(ILifetimeScope container)
            :base(container)
        {            
            logger = container.Resolve<ILogger<LanguageProvider>>();
        }

        //Custom Field Declaration
        public List<Language> GetLanguages()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Language> returnData = context.Language
                                .ToList();

                    return new List<Language>(returnData); 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public List<Language> GetActiveLanguages()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Language> returnData = context.Language
                        .Where(l=>l.IsActive)
                        .ToList();

                    return new List<Language>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting active languages", ex);
                throw ex;
            }
        }

        public Language GetLanguage(Guid languageId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Language returnData = context.Language
                              .Where(e => e.Id == languageId)
                              .FirstOrDefault();

                    return returnData; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public Language CreateLanguage(Language language)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Language resultLayout;
                    language.CreatedDate = language.LastModifiedDate = DateTime.Now;
                    language.IsActive = true;

                    resultLayout = context.Language.Add(language).Entity;
                    context.SaveChanges();
                    return resultLayout; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating Language", ex);
            }
            return null;
        }
        public Language UpdateLanguage(Language language)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    Language resultLayout;
                    resultLayout = context.Language.Attach(language).Entity;
                    context.Entry(language).State = EntityState.Modified;
                    context.SaveChanges();
                    return resultLayout; 
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating Language", ex);
            }
            return null;
        }

        public bool IsMultilingual()
        {
            var result = GetActiveLanguages();
            return result != null && result.Count > 1;
        }
    }
}
