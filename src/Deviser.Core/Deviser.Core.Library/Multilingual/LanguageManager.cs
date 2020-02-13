using Deviser.Core.Data.Repositories;
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
using Deviser.Core.Common.FileProviders;
using Deviser.Core.Data.Installation;

namespace Deviser.Core.Library.Multilingual
{
    public class LanguageManager : ILanguageManager
    {
        //Logger
        private readonly ILogger<LanguageManager> _logger;
        private readonly ILanguageRepository _languageRepository;
        private readonly INavigation _navigation;

        public LanguageManager(ILogger<LanguageManager> logger,
            ILanguageRepository languageRepository,
            INavigation navigation)
        {
            _logger = logger;
            _languageRepository = languageRepository;
            _navigation = navigation;
        }

        public List<Language> GetAllLanguages(bool exceptEnabled = false)
        {
            try
            {
                //string culuresJsonPath = Path.Combine(_hostingEnvironment.ContentRootPath, "cultures.json");
                var json = EmbeddedProvider.GetFileContentAsString(typeof(DataSeeder).Assembly, "Cultures.json");
                List<Language> cultures = SDJsonConvert.DeserializeObject<List<Language>>(json);

                cultures.ForEach(c => c.FallbackCulture = Globals.FallbackLanguage);
                
                if (exceptEnabled)
                {
                    var enabledLanguages = _languageRepository.GetLanguages();
                    cultures = cultures.Where(language => !enabledLanguages.Any(el => el.CultureCode == language.CultureCode)).ToList();
                }

                return cultures;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all languages", ex);
                throw ex;
            }
        }

        public List<Language> GetActiveLanguages()
        {
            try
            {
                var enabledLanguages = _languageRepository.GetActiveLanguages();
                return enabledLanguages;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting active languages", ex);
                throw ex;
            }
        }

        public List<Language> GetLanguages()
        {
            try
            {
                var result = _languageRepository.GetLanguages();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting languages", ex);
                throw ex;
            }
        }

        public Language GetLanguage(Guid languageId)
        {
            try
            {
                var result = _languageRepository.GetLanguage(languageId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting a language", ex);
                throw ex;
            }
        }

        public Language CreateLanguage(Language language)
        {
            try
            {
                var before = _languageRepository.IsMultilingual();
                var result = _languageRepository.CreateLanguage(language);
                var after = _languageRepository.IsMultilingual();
                if (before != after)
                {
                    TranslateAllPages();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating a language", ex);
                throw ex;
            }
        }

        public Language UpdateLanguage(Language language)
        {
            try
            {
                var before = _languageRepository.IsMultilingual();
                var result = _languageRepository.UpdateLanguage(language);
                var after = _languageRepository.IsMultilingual();
                if(before!=after)
                {
                    TranslateAllPages();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating languages", ex);
                throw ex;
            }
        }
        
        /// <summary>
        /// Update URL in page translation when site is not multilingual
        /// </summary>
        private void TranslateAllPages()
        {
            var root = _navigation.GetPageTree();
            _navigation.UpdatePageTree(root);
        }
    }
}
