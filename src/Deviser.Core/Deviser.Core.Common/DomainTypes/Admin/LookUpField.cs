using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
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
