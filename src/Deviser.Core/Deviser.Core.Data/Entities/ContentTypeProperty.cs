using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class ContentTypeProperty
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid ConentTypeId { get; set; }
        public virtual Property Property { get; set; }
        public virtual ContentType ContentType { get; set; }
    }
}
