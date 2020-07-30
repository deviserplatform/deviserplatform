using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ModuleView
    {
        public ModuleView()
        {
            ModuleActionProperty = new HashSet<ModuleViewProperty>();
            PageModule = new HashSet<PageModule>();
        }

        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ControllerNamespace { get; set; }
        public string DisplayName { get; set; }
        public string IconClass { get; set; }
        public string IconImage { get; set; }
        public bool IsDefault { get; set; }
        public Guid ModuleActionTypeId { get; set; }
        public Guid ModuleId { get; set; }

        public virtual Module Module { get; set; }
        public virtual ModuleViewType ModuleViewType { get; set; }
        public virtual ICollection<ModuleViewProperty> ModuleActionProperty { get; set; }
        public virtual ICollection<PageModule> PageModule { get; set; }
    }
}
