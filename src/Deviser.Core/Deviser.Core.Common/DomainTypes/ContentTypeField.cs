using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class ContentTypeField
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public string FieldDescription { get; set; }
        public Guid ContentTypeId { get; set; }
        public Guid ContentFieldTypeId { get; set; }
        public int SortOrder { get; set; }
        public bool IsShownOnList { get; set; }
        public bool IsShownOnPreview { get; set; }
        public ContentFieldType ContentFieldType { get; set; }
        public string ContentFieldTypeName => ContentFieldType?.Name;
    }
}
