using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ContentControlProperty
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid ContentControlId { get; set; }
        public virtual Property Property { get; set; }
        public virtual ContentControl ContentControl { get; set; }
    }
}
