using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentControl
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public Guid FieldTypeId { get; set; }       
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
