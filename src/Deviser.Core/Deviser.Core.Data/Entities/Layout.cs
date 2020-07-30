using System;
using System.Collections.Generic;

namespace Deviser.Core.Data.Entities
{
    public partial class Layout
    {
        public Layout()
        {
            Page = new HashSet<Page>();
        }

        public Guid Id { get; set; }
        public string Config { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Page> Page { get; set; }
    }
}
