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

        public int Id { get; set; }
        public string Config { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Page> Page { get; set; }
    }
}
