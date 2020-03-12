using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class OptionList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public List<PropertyOption> List { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string IsActiveText => IsActive ? "Active" : "In Active";
        public string IsActiveBadgeClass => IsActive ? "badge-primary" : "badge-secondary";
    }
}
