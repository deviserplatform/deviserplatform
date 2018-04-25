using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class ModuleAction
    {
        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ControllerNamespace { get; set; }
        public string DisplayName { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public Guid ModuleActionTypeId { get; set; }
        public Guid ModuleId { get; set; }
        public bool IsDefault { get; set; }
        public virtual ModuleActionType ModuleActionType { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<PageModule> PageModules { get; set; }
        public ICollection<ModuleActionProperty> ModuleActionProperties { get; set; }
    }
}
