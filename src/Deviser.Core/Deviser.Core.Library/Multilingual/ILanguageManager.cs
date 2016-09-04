using System.Collections.Generic;
using Deviser.Core.Data.Entities;
using System;

namespace Deviser.Core.Library.Multilingual
{
    public interface ILanguageManager
    {
        Language CreateLanguage(Language language);
        List<Language> GetActiveLanguages();
        List<Language> GetAllLanguages(bool exceptEnabled = false);
        List<Language> GetLanguages();
        Language GetLanguage(Guid languageId);        
        Language UpdateLanguage(Language language);
    }
}