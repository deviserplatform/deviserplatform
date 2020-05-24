using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class ModuleViewType
    {
        public ModuleViewType()
        {
            ModuleView = new HashSet<ModuleView>();
        }

        public Guid Id { get; set; }
        public string ControlType { get; set; }

        public virtual ICollection<ModuleView> ModuleView { get; set; }
    }
}
