using System.Collections.Generic;

namespace Deviser.Core.Library.Layouts
{
    public interface ISkinManager
    {
        List<KeyValuePair<string, string>> GetHostSkins();
    }
}