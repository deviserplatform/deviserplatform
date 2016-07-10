using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common;

namespace Deviser.Core.Library.Multilingual
{
    public class LanguageManager : ILanguageManager
    {
        //Logger
        private readonly ILogger<LanguageManager> logger;
        private ILanguageProvider languageProvider;
        private IHostingEnvironment hostingEnvironment;

        public LanguageManager(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LanguageManager>>();
            languageProvider = container.Resolve<ILanguageProvider>();
            hostingEnvironment = container.Resolve<IHostingEnvironment>();
        }

        public List<Language> GetAllLanguages(bool exceptEnabled = false)
        {
            try
            {
                string culuresJsonPath = hostingEnvironment.ContentRootPath + "\\cultures.json";
                List<Language> cultures = SDJsonConvert.DeserializeObject<List<Language>>(File.ReadAllText(culuresJsonPath));

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
                var enabledLanguages = languageProvider.GetLanguages();
                enabledLanguages = enabledLanguages.Where(l => l.IsActive).ToList();
                return enabledLanguages;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting active languages", ex);
                throw ex;
            }
        }
    }
}
