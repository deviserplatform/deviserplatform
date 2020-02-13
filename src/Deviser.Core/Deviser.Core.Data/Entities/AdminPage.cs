using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class AdminPage
    {
        public Guid ModuleId { get; set; }
        public Guid PageId { get; set; }
        public string ModelName { get; set; }

        public Page Page { get; set; }
        public Module Module { get; set; }
    }
}
