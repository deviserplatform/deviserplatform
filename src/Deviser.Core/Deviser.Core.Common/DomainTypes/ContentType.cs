using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentType
    {
        public Guid Id { get; set; }

        [BindRequired]
        public string Name { get; set; }

        [BindRequired]
        public string Label { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public int SortOrder { get; set; }
        public ICollection<Property> Properties { get; set; }
        public ICollection<ContentTypeField> ContentTypeFields { get; set; }
        public bool IsActive { get; set; }
        //public string IsActiveText => IsActive ? "Active" : "In Active";
        public string IsActiveBadgeClass => IsActive ? "badge-primary" : "badge-secondary";

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
