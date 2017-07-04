using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Entities
{
    public class Validator
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string RegExp { get; set; }
        public Guid FieldTypeId { get; set; }
        public FieldType FieldType { get; set; }
        public ICollection<ContentTypeControl> ContentTypeControls { get; set; }

    }
}
