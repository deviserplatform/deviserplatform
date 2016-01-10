using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class ModuleAction
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ControllerNamespace { get; set; }
        public string DisplayName { get; set; }
        public int ModuleActionTypeId { get; set; }
        public int ModuleId { get; set; }
        public bool IsDefault { get; set; }
        public virtual ModuleActionType ModuleActionType { get; set; }
        public virtual Module Module { get; set; }
    }
}
