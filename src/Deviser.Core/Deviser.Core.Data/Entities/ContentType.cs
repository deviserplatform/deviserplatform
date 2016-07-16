using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Entities
{
    public class ContentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } //old property - Type        
        public string Label { get; set; }
        public string IconImage { get; set; }
        public string IconClass { get; set; }
        public int SortOrder { get; set; }
        public Guid ContentDataTypeId { get; set; }
        public ContentDataType ContentDataType { get; set; }
        public ICollection<ContentTypeProperty> ContentTypeProperties { get; set; }
        public ICollection<PageContent> PageContents { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
