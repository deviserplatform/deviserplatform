using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class Module
    {
        public Module()
        {
            ModuleView = new HashSet<ModuleView>();
            PageModule = new HashSet<PageModule>();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Label { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public virtual ICollection<ModuleView> ModuleView { get; set; }
        public virtual ICollection<PageModule> PageModule { get; set; }
        public virtual ICollection<AdminPage> AdminPage { get; set; }

    }
}
