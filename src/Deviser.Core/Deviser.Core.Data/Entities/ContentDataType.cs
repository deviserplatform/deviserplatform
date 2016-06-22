using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class ContentDataType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public ICollection<ContentType> ContentTypes { get; set; }

    }
}
