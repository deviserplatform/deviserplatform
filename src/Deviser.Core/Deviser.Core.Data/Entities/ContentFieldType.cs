using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class ContentFieldType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public ICollection<ContentTypeField> ContentTypeFields { get; set; }

    }
}
