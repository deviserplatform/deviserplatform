using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ModuleViewType
    {
        public ModuleViewType()
        {
            ModuleAction = new HashSet<ModuleView>();
        }

        public Guid Id { get; set; }
        public string ControlType { get; set; }

        public virtual ICollection<ModuleView> ModuleAction { get; set; }
    }
}
