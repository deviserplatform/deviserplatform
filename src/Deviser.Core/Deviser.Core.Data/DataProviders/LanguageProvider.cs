using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;

namespace Deviser.Core.Data.DataProviders
{
    public interface ILanguageProvider
    {
        List<Language> GetLanguages();
        Language GetLanguage(Guid languageId);
        Language CreateLanguage(Language language);
        Language UpdateLanguage(Language language);

    }

    public class LanguageProvider : ILanguageProvider
    {
        //Logger
        private readonly ILogger<LanguageProvider> logger;
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public LanguageProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LanguageProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration
        public List<Language> GetLanguages()
        {
            try
            {
                IEnumerable<Language> returnData = context.Language
                    .ToList();

                return new List<Language>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Language", ex);
            }
            return null;
        }

        public Language GetLanguage(Guid languageId)
        {
            try
            {

                Language returnData = context.Language
                   .Where(e => e.Id == languageId)
                   .FirstOrDefault();

                return returnData;
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
                Language resultLayout;
                resultLayout = context.Language.Add(language, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultLayout;
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
                Language resultLayout;
                resultLayout = context.Language.Attach(language, GraphBehavior.SingleObject).Entity;
                context.Entry(language).State = EntityState.Modified;
                context.SaveChanges();
                return resultLayout;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating Language", ex);
            }
            return null;
        }
    }
}
