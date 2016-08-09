using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class Module
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public virtual ICollection<ModuleAction> ModuleAction { get; set; }
        public virtual ICollection<PageModule> PageModule { get; set; }
    }
}
