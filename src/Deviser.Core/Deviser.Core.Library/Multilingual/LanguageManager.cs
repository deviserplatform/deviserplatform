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
            var json = EmbeddedProvider.GetFileContentAsString(typeof(LanguageManager).Assembly, "Cultures.json");
            var cultures = SDJsonConvert.DeserializeObject<List<Language>>(json);

            cultures.ForEach(c => c.FallbackCulture = Globals.FallbackLanguage);

            if (!exceptEnabled) return cultures;

            var enabledLanguages = _languageRepository.GetLanguages();
            cultures = cultures.Where(language => enabledLanguages.All(el => el.CultureCode != language.CultureCode)).ToList();

            return cultures;
        }

        public List<Language> GetActiveLanguages()
        {
            var enabledLanguages = _languageRepository.GetActiveLanguages();
            return enabledLanguages;
        }

        public List<Language> GetLanguages()
        {
            var result = _languageRepository.GetLanguages();
            return result;
        }

        public Language GetLanguage(Guid languageId)
        {
            var result = _languageRepository.GetLanguage(languageId);
            return result;
        }

        public Language CreateLanguage(Language language)
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

        public Language UpdateLanguage(Language language)
        {
            var before = _languageRepository.IsMultilingual();
            var result = _languageRepository.UpdateLanguage(language);
            var after = _languageRepository.IsMultilingual();
            if (before != after)
            {
                TranslateAllPages();
            }
            return result;
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
