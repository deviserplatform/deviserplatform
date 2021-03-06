﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageModule
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ContainerId { get; set; }
        public bool IsActive { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ModuleViewId { get; set; }
        public int SortOrder { get; set; }
        public Guid PageId { get; set; }
        public ModuleView ModuleView { get; set; }
        public  Module Module { get; set; }
        public virtual Page Page { get; set; }
        public bool InheritViewPermissions { get; set; }
        public bool InheritEditPermissions { get; set; }
        public bool HasEditPermission { get; set; }
        public ICollection<Property> Properties { get; set; }
        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
    }
}
