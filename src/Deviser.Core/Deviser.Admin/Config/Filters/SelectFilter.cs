using System.Collections.Generic;

namespace Deviser.Admin.Config.Filters
{
    public class SelectFilter : Filter
    {
        public IList<string> FilterKeys { get; set; }
    }
}
