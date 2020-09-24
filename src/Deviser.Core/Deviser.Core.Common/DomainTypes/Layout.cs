using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class Layout
    {
        public Guid Id { get; set; }
        public string Config { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
