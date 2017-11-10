using System.Collections.Generic;

namespace Deviser.Core.Library.Layouts
{
    public interface IThemeManager
    {
        List<KeyValuePair<string, string>> GetHostThemes();
    }
}