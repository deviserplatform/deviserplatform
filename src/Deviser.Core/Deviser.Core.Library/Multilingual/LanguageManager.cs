using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common;
using System.Globalization;

namespace Deviser.Core.Library.Multilingual
{
    public class LanguageManager : ILanguageManager
    {
        //Logger
        private readonly ILogger<LanguageManager> logger;
        private ILanguageProvider languageProvider;
        private INavigation navigation;
        private IHostingEnvironment hostingEnvironment;

        public LanguageManager(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LanguageManager>>();
            languageProvider = container.Resolve<ILanguageProvider>();
            hostingEnvironment = container.Resolve<IHostingEnvironment>();
            navigation = container.Resolve<INavigation>();
        }

        public List<Language> GetAllLanguages(bool exceptEnabled = false)
        {
            try
            {
                string culuresJsonPath = hostingEnvironment.ContentRootPath + "\\cultures.json";
                List<Language> cultures = SDJsonConvert.DeserializeObject<List<Language>>(System.IO.File.ReadAllText(culuresJsonPath));

                cultures.ForEach(c => c.FallbackCulture = Globals.FallbackLanguage);

                //var result = CultureInfo.GetCultureInfo(System.Globalization.CultureTypes.SpecificCultures).OrderBy(c => c.NativeName)
                //           .Select(c => new Language
                //           {
                //               EnglishName = c.EnglishName,
                //               NativeName = c.NativeName,
                //               CultureCode = c.Name,
                //               FallbackCulture = Globals.FallbackLanguage
                //           })
                //           .ToList();

                if (exceptEnabled)
                {
                    var enabledLanguages = languageProvider.GetLanguages();
                    cultures = cultures.Where(language => !enabledLanguages.Any(el => el.CultureCode == language.CultureCode)).ToList();
                }

                return cultures;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting all languages", ex);
                throw ex;
            }
        }

        public List<Language> GetActiveLanguages()
        {
            try
            {
                var enabledLanguages = languageProvider.GetActiveLanguages();
                return enabledLanguages;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting active languages", ex);
                throw ex;
            }
        }

        public List<Language> GetLanguages()
        {
            try
            {
                var result = languageProvider.GetLanguages();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting languages", ex);
                throw ex;
            }
        }

        public Language GetLanguage(Guid languageId)
        {
            try
            {
                var result = languageProvider.GetLanguage(languageId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting a language", ex);
                throw ex;
            }
        }

        public Language CreateLanguage(Language language)
        {
            try
            {
                var before = languageProvider.IsMultilingual();
                var result = languageProvider.CreateLanguage(language);
                var after = languageProvider.IsMultilingual();
                if (before != after)
                {
                    TranslateAllPages();
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating a language", ex);
                throw ex;
            }
        }

        public Language UpdateLanguage(Language language)
        {
            try
            {
                var before = languageProvider.IsMultilingual();
                var result = languageProvider.UpdateLanguage(language);
                var after = languageProvider.IsMultilingual();
                if(before!=after)
                {
                    TranslateAllPages();
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating languages", ex);
                throw ex;
            }
        }
        
        /// <summary>
        /// Update URL in page translation when site is not multilingual
        /// </summary>
        private void TranslateAllPages()
        {
            var root = navigation.GetPageTree();
            navigation.UpdatePageTree(root);
        }
    }
}
