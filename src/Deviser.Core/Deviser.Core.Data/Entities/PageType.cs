using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class PageType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Page> Page { get; set; }
    }
}
