using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class LayoutType
    {
        public LayoutType()
        {
            LayoutTypeProperty = new HashSet<LayoutTypeProperty>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string IconClass { get; set; }
        public string IconImage { get; set; }
        public bool? IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LayoutTypeIds { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LayoutTypeProperty> LayoutTypeProperty { get; set; }
    }
}
