using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class Property
    {
        public Property()
        {
            ContentTypeProperty = new HashSet<ContentTypeProperty>();
            LayoutTypeProperty = new HashSet<LayoutTypeProperty>();
            ModuleActionProperty = new HashSet<ModuleActionProperty>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }
        public Guid? OptionListId { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }

        public virtual OptionList OptionList { get; set; }
        public virtual ICollection<ContentTypeProperty> ContentTypeProperty { get; set; }
        public virtual ICollection<LayoutTypeProperty> LayoutTypeProperty { get; set; }
        public virtual ICollection<ModuleActionProperty> ModuleActionProperty { get; set; }
    }
}
