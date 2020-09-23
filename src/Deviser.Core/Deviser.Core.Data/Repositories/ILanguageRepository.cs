using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

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
}