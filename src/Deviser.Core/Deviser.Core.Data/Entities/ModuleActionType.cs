using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class ModuleActionType
    {
        public ModuleActionType()
        {
            ModuleAction = new HashSet<ModuleAction>();
        }

        public int Id { get; set; }
        public string ControlType { get; set; }

        public virtual ICollection<ModuleAction> ModuleAction { get; set; }
    }
}
