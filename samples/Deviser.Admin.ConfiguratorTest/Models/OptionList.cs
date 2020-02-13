using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class OptionList
    {
        public OptionList()
        {
            Property = new HashSet<Property>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string List { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Property> Property { get; set; }
    }
}
