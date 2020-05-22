using System.Collections.Generic;

namespace Deviser.Admin.Config.Filters
{
    public class SelectFilter : Filter
    {
        public string KeyFieldName { get; set; }
        public IList<string> FilterKeyValues { get; set; }
    }
}
