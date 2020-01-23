using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class AdminPage
    {
        public Guid ModuleId { get; set; }
        public Guid PageId { get; set; }
        public string ModelName { get; set; }
        public string ModuleName { get; set; }
    }
}
