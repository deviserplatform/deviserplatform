using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class DynamicContent
    {
        public PageContent PageContent { get; set; }

        public dynamic Content { get; set; }
    }
}
