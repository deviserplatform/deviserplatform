using System.Collections.Generic;

namespace Deviser.Admin.Config
{
    public class LookUpField
    {
        public Dictionary<string, object> Key { get; set; }
        public string DisplayName { get; set; }

        public LookUpField()
        {
            Key = new Dictionary<string, object>();
        }
    }
}
