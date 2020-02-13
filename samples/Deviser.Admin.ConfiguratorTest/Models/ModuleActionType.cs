using System;
using System.Collections.Generic;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class ModuleActionType
    {
        public ModuleActionType()
        {
            ModuleAction = new HashSet<ModuleAction>();
        }

        public Guid Id { get; set; }
        public string ControlType { get; set; }

        public virtual ICollection<ModuleAction> ModuleAction { get; set; }
    }
}
