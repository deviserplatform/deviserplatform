using System.Collections.Generic;
using Deviser.Core.Data.Entities;

namespace Deviser.Core.Library.Multilingual
{
    public interface ILanguageManager
    {
        List<Language> GetActiveLanguages();
        List<Language> GetAllLanguages(bool exceptEnabled = false);
    }
}