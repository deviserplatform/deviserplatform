﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class ModuleView
    {
        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ControllerNamespace { get; set; }
        public string DisplayName { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public Guid ModuleViewTypeId { get; set; }
        public Guid ModuleId { get; set; }
        public bool IsDefault { get; set; }
        public virtual ModuleViewType ModuleViewType { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}
